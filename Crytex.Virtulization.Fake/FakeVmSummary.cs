using System;
using System.Collections.Generic;
using Crytex.Virtualization.Base;
using Crytex.Virtualization.Base.InfoAboutVM;

namespace Crytex.Virtulization.Fake
{
    public class FakeVmSummary : ISummary
    {
        public BaseInfo BaseInformation
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<DriveInfo> Drives
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public GuestOSState GuestOSState
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<NetworkInfo> Networks
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public PowerState PowerStateMachine
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateSummary(object newInfo, FlagsUpdateInfo flags)
        {
            throw new NotImplementedException();
        }
    }
}
