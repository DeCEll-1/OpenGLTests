using RGL.API.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RGL.API.Helpers
{
    internal class GCLoop
    {
        private CancellationTokenSource? _gcLoopCts;

        public void StartGcLoop()
        {
            _gcLoopCts = new CancellationTokenSource();
            _ = RunGcLoopAsync(_gcLoopCts.Token);
        }

        public void StopGcLoop()
        {
            _gcLoopCts?.Cancel();
            _gcLoopCts = null;
        }

        private async Task RunGcLoopAsync(CancellationToken token)
        {

            await Task.Delay(TimeSpan.FromSeconds(1), token);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (GC.GetTotalMemory(forceFullCollection: false) < APISettings.MinRamBytesForForcedGC)
                    {
                        if (APISettings.LogForceGC)
                            Logger.Log($"Current Memory Usage: {LogColors.BR(Logger.FormatBytes(GC.GetTotalMemory(false)))}", LogLevel.Detail);
                    }
                    else
                    {// so, basically,
                        // for god knows why, the garbage from image datas dont get directly collected
                        // i dont know why, they *do* get collected when im profiling after some time
                        // however even when collected task manager displays like 40mb ram usage min
                        // im giving it to the CRL, because it prints less than 1mb of ram usage here (currently)

                        if (APISettings.LogForceGC)
                            Logger.Log($"Current Memory Usage: {LogColors.BR(Logger.FormatBytes(GC.GetTotalMemory(false)))}", LogLevel.Detail);

                        Logger.BeginMemoryBlock();

                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
                        GC.WaitForPendingFinalizers();

                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.Default;

                        if (APISettings.LogForceGC)
                        {
                            Logger.Log($"Forced GC and LOH compaction ran saving: {LogColors.BR(Logger.FormatBytes(-Logger.EndMemoryBlock()) + " RAM.")} ", LogLevel.Detail);
                            Logger.Log($"Total GC Runs [0, 1, 2]: {LogColors.BR(GC.CollectionCount(0) + ", " + GC.CollectionCount(1) + ", " + GC.CollectionCount(2))}", LogLevel.Detail);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Log($"Exception in GC loop: {ex}", LogLevel.Error);
                }

                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(APISettings.ForceGCIntervalMS), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

    }
}
