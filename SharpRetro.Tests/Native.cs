using NUnit.Framework;
using SharpRetro.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpRetro.Tests
{
  public class Native
  {
    [Test]
    public void TestStringAllocation()
    {
      string testString = "test string";
      SafeStringHandle handle = new SafeStringHandle(testString);
      string deallocated = Marshal.PtrToStringAnsi(handle.DangerousGetHandle());
      handle.Dispose();
      Assert.AreEqual(testString, deallocated);
      Assert.IsTrue(handle.IsClosed);
    }
  }
}
