using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace XiangJiang.Windows.Core
{
    public sealed class SingleInstance : IDisposable
    {
        private readonly bool _acrossSessions;
        private readonly uint _timeout;
        public bool _hasHandle;
        private string _instanceName;
        private Mutex _mutex;

        public SingleInstance(string instanceName, bool acrossSessions = false, uint timeout = 0)
        {
            _instanceName = instanceName;
            _acrossSessions = acrossSessions;
            _timeout = timeout;
        }

        public void Dispose()
        {
            if (_mutex != null)
            {
                if (_hasHandle)
                    _mutex.ReleaseMutex();
                _mutex.Close();
            }
        }

        private bool CreateMutex()
        {
            if (_acrossSessions)
            {
                _instanceName = $"Global\\{_instanceName}";
                var allowEveryoneRule =
                    new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid
                            , null)
                        , MutexRights.Modify | MutexRights.Synchronize
                        , AccessControlType.Allow
                    );
                var securitySettings = new MutexSecurity();
                securitySettings.AddAccessRule(allowEveryoneRule);
                _mutex.SetAccessControl(securitySettings);
            }

            _mutex = new Mutex(false, _instanceName, out var createNew);
            return createNew;
        }

        public bool Acquire()
        {
            try
            {
                CreateMutex();
                if (_timeout == 0)
                    _hasHandle = _mutex.WaitOne(Timeout.Infinite, false);
                else
                    _hasHandle = _mutex.WaitOne((int) _timeout, false);

                if (_hasHandle == false)
                    throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
            }
            catch (AbandonedMutexException)
            {
                _hasHandle = true;
            }
        }
    }
}