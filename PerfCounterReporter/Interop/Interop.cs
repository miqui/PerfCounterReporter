﻿using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace PerfCounterReporter.Interop
{
    internal sealed class PdhSafeDataSourceHandle : SafeHandle
    {
        private PdhSafeDataSourceHandle()
            : base(IntPtr.Zero, true)
        { }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return (Interop.PdhCloseLog(base.handle, 0) == 0);
        }

        public override bool IsInvalid
        {
            get { return (base.handle == IntPtr.Zero); }
        }
    }

    internal class Interop
    {
        [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
        public static extern uint PdhBindInputDataSource(out PdhSafeDataSourceHandle phDataSource, string szLogFileNameList);
        [DllImport("pdh.dll")]
        internal static extern uint PdhCloseLog(IntPtr logHandle, uint dwFlags);
        [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
        public static extern uint PdhExpandWildCardPathHW(PdhSafeDataSourceHandle hDataSource, string szWildCardPath, IntPtr mszExpandedPathList, ref IntPtr pcchPathListLength, uint dwFlags);
        [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
        public static extern uint PdhValidatePathEx(PdhSafeDataSourceHandle hDataSource, string szFullPathBuffer);
        [DllImport("pdh.dll", CharSet = CharSet.Unicode)]
        public static extern uint PdhParseCounterPath(string szFullPathBuffer, IntPtr pCounterPathElements, ref IntPtr pdwBufferSize, uint dwFlags);
    }
}
