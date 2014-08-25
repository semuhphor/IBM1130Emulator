using Microsoft.VisualStudio.TestTools.UnitTesting;
using S1130.SystemObjects;
using S1130.SystemObjects.Instructions;

namespace UnitTests.S1130.SystemObjects.InstructionTests
{
	[TestClass]
	public class AddDoubleTests : InstructionTestBase
	{
		[TestMethod]
		public void Exeucte_AD_Short_NoTag_Positive_NoCarryNoOverflow()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 0, 0x0011);
			InsCpu.NextInstruction();
			InsCpu[InsCpu.Iar + 0x0011] = 0x0001;
			InsCpu[InsCpu.Iar + 0x0012] = 0x0002;
			ExecAndTest(initialAcc: 0x0003, initialExt: 0x0007, initialCarry: false, initialOverflow: false, expectedAcc: 0x0004, expectedExt: 0x0009, expectedCarry: false, expectedOverflow: false);
		}

		[TestMethod]
		public void Exeucte_AD_Short_NoTag_Positive_OverflowNotReset()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 0, 0x0011);
			InsCpu.NextInstruction();
			InsCpu[InsCpu.Iar + 0x0011] = 0x0001;
			InsCpu[InsCpu.Iar + 0x0012] = 0x0002;
			ExecAndTest(initialAcc: 0x0003, initialExt: 0x0007, initialCarry: false, initialOverflow: true, expectedAcc: 0x0004, expectedExt: 0x0009, expectedCarry: false, expectedOverflow: true);
		}

		[TestMethod]
		public void Exeucte_AD_Short_NoTag_Positive_NoCarryNoOverflow_OddAddress()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 0, 0x0010);
			InsCpu.NextInstruction();
			InsCpu[InsCpu.Iar + 0x0010] = 0x0003;
			ExecAndTest(initialAcc: 0x0004, initialExt: 0x0002, initialCarry: false, initialOverflow: false, expectedAcc: 0x0007, expectedExt: 0x0005, expectedCarry: false, expectedOverflow: false);
		}

		[TestMethod]
		public void Execute_AD_Short_NoTag_OverflowNoCarry()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 0, 0x0011);
			InsCpu.NextInstruction();
			InsCpu[InsCpu.Iar + 0x0011] = 0x4000;
			InsCpu[InsCpu.Iar + 0x0012] = 0x0000;
			ExecAndTest(initialAcc: 0x4000, initialExt: 0x0000, initialCarry: false, initialOverflow: false, expectedAcc: 0x8000, expectedExt: 0x0000, expectedCarry: false, expectedOverflow: true);
		}

		[TestMethod]
		public void Execute_AD_Short_NoTag_Negative_CarryNoOverflow()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 0, 0x21);
			InsCpu.NextInstruction();
			InsCpu[InsCpu.Iar + 0x21] = 0xffff;
			InsCpu[InsCpu.Iar + 0x22] = 0xffff;
			ExecAndTest(initialAcc: 0x0000, initialExt:0x0001, initialCarry: false, initialOverflow: false, expectedAcc: 0x0000, expectedExt:0x0000, expectedCarry: true, expectedOverflow: false);
		}

		[TestMethod]
		public void Execute_AD_Short_Xr2_Positive_NoCarryNoOverflow()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 2, 0x0024);
			InsCpu.NextInstruction();
			InsCpu.Xr[2] = 0x0250;
			InsCpu[InsCpu.Xr[2] + 0x0024] = 0x0fff;
			InsCpu[InsCpu.Xr[2] + 0x0025] = 0xffff;
			ExecAndTest(initialAcc: 0x0000, initialExt:0x0001, initialCarry: false, initialOverflow: false, expectedAcc: 0x1000, expectedExt:0x0000, expectedCarry: false, expectedOverflow: false);
		}

		[TestMethod]
		public void Execute_AD_Short_Xr2_Negative_CarryAndOverflow()
		{
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.AddDouble, 2, 0x0024);
			InsCpu.NextInstruction();
			InsCpu.Xr[2] = 0x0250;
			InsCpu[InsCpu.Xr[2] + 0x0024] = 0x8000;
			InsCpu[InsCpu.Xr[2] + 0x0025] = 0x0000;
			ExecAndTest(initialAcc: 0x8000, initialExt:0x0000, initialCarry: false, initialOverflow: false, expectedAcc: 0x0000, expectedExt:0x0000, expectedCarry: true, expectedOverflow: true);
		}

		[TestMethod]
		public void Exeucte_AD_Long_NoTag_Positive_NoCarryNpOverflow()
		{
			InstructionBuilder.BuildLongAtIar(OpCodes.AddDouble, 0, 0x400, InsCpu);
			InsCpu.NextInstruction();
			InsCpu[0x0400] = 0x0001;
			InsCpu[0x0401] = 0x0002;
			ExecAndTest(initialAcc: 0x0003, initialExt: 0x0007, initialCarry: false, initialOverflow: false, expectedAcc: 0x0004, expectedExt: 0x0009, expectedCarry: false, expectedOverflow: false);
		}

		[TestMethod]
		public void Exeucte_AD_Long_Xr3_Negative_CarryNoOverflow()
		{
			InstructionBuilder.BuildLongAtIar(OpCodes.AddDouble, 3, 0x400, InsCpu);
			InsCpu.NextInstruction();
			InsCpu.Xr[3] = 0x0100;
			InsCpu[0x0500] = 0xffff;
			InsCpu[0x0501] = 0xf100;
			ExecAndTest(initialAcc: 0x0000, initialExt: 0x0fff, initialCarry: false, initialOverflow: false, expectedAcc: 0x0000, expectedExt: 0x00ff, expectedCarry: true, expectedOverflow: false);
		}

		[TestMethod]
		public void Exeucte_AD_Indirect_Xr3_Positive_NoCarryNoOverflow()
		{
			InstructionBuilder.BuildLongIndirectAtIar(OpCodes.AddDouble, 3, 0x400, InsCpu);
			InsCpu.NextInstruction();
			InsCpu.Xr[3] = 0x0050;
			InsCpu[0x0450] = 0x500;
			InsCpu[0x0500] = 0x0020;
			InsCpu[0x0501] = 0x0040;
			ExecAndTest(initialAcc: 0x0001, initialExt: 0x0002, initialCarry: false, initialOverflow: false, expectedAcc: 0x0021, expectedExt: 0x0042, expectedCarry: false, expectedOverflow: false);
		}

		private void ExecAndTest(ushort expectedAcc, ushort expectedExt, bool expectedCarry, bool expectedOverflow, ushort initialAcc, ushort initialExt, bool initialCarry, bool initialOverflow)
		{
			InsCpu.Acc = initialAcc;
			InsCpu.Ext = initialExt;
			InsCpu.Carry = initialCarry;
			InsCpu.Overflow = initialOverflow;
			InsCpu.ExecuteInstruction();
			Assert.AreEqual(expectedAcc, InsCpu.Acc);
			Assert.AreEqual(expectedExt, InsCpu.Ext);
			Assert.AreEqual(expectedCarry, InsCpu.Carry);
			Assert.AreEqual(expectedOverflow, InsCpu.Overflow);
		}
	}
}