using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace XiangJiang.Windows.Core
{
    public sealed class SingleGlobalInstance : IDisposable
    {
        private readonly bool _crossSessions;
        private bool _hasHandle;
        private string _instanceName;
        private Mutex _mutex;

        public SingleGlobalInstance(string instanceName, bool crossSessions = false)
        {
            _instanceName = instanceName;
            _crossSessions = crossSessions;
            CreateMutex();
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
            if (_crossSessions)
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

        public bool Acquire(uint timeout = 0)
        {
            _hasHandle = timeout == 0 ? _mutex.WaitOne(Timeout.Infinite, false) : _mutex.WaitOne((int) timeout, false);
            return _hasHandle;
        }
    }
}