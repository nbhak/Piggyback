// Decompiled with JetBrains decompiler
// Type: Utility.MiniDumpType
// Assembly: Piggyback3, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: C45B2B95-8862-4F73-BBB7-A401C9603E2E
// Assembly location: C:\Program Files (x86)\Developmental Optometry\Piggyback 3\Piggyback3.exe

namespace Utility
{
  public enum MiniDumpType
  {
    Normal = 0,
    WithDataSegs = 1,
    WithFullMemory = 2,
    WithHandleData = 4,
    FilterMemory = 8,
    ScanMemory = 16, // 0x00000010
    WithUnloadedModules = 32, // 0x00000020
    WithIndirectlyReferencedMemory = 64, // 0x00000040
    FilterModulePaths = 128, // 0x00000080
    WithProcessThreadData = 256, // 0x00000100
    WithPrivateReadWriteMemory = 512, // 0x00000200
    WithoutOptionalData = 1024, // 0x00000400
    WithFullMemoryInfo = 2048, // 0x00000800
    WithThreadInfo = 4096, // 0x00001000
    WithCodeSegs = 8192, // 0x00002000
    WithoutAuxiliaryState = 16384, // 0x00004000
    WithFullAuxiliaryState = 32768, // 0x00008000
  }
}
