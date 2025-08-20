using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    partial class MITSUBISHIDataUpdater : BaseUpdater
    {
        public const string _name = ControllerNames.MITSUBISHI;

        public MITSUBISHIDataUpdater()
        {
            cu = new MITSUBISHIControllerUtilities();
        }

        public MITSUBISHIDataUpdater(MITSUBISHIControllerUtilities _cu)
        {
            cu = _cu;
        }

        public override void Connect()
        {
            // Implementation for connecting to MITSUBISHI devices
            base.Connect();
        }

        public override void Update()
        {
            // Implementation for updating data from MITSUBISHI devices
            base.Update();
        }

        public override void Dispose()
        {
            if (cu != null)
            {
                (cu as MITSUBISHIControllerUtilities).CloseComm();
            }
            base.Dispose();
        }
    }
}