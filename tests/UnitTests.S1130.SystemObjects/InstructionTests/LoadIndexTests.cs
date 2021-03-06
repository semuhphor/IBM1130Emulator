﻿using Xunit;
using S1130.SystemObjects.Instructions;

namespace UnitTests.S1130.SystemObjects.InstructionTests
{
    
    public class LoadIndexTests : InstructionTestBase
    {
		[Fact]
		public void Execute_LDX_Short_PositiveDisplacement()
		{
			BeforeEachTest();
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.LoadIndex, 1, 0x10);
			InsCpu.NextInstruction();
			InsCpu.ExecuteInstruction();
			Assert.Equal(0x10, InsCpu.Xr[1]);
		}

		[Fact]
		public void Execute_LDX_Short_NegativeDisplacement()
		{
			BeforeEachTest();
			InsCpu.AtIar = InstructionBuilder.BuildShort(OpCodes.LoadIndex, 3, 0x80);
			InsCpu.NextInstruction();
			InsCpu.ExecuteInstruction();
			Assert.Equal(0xff80, InsCpu.Xr[3]);
		}

		[Fact]
		public void Execute_LDX_Long_NoTag()
		{
			BeforeEachTest();
			InstructionBuilder.BuildLongAtIar(OpCodes.LoadIndex, 0, 0x404, InsCpu);
			InsCpu.NextInstruction();
			InsCpu.ExecuteInstruction();
			Assert.Equal(0x404, InsCpu.Iar);
			Assert.Equal(0x404, InsCpu.Xr[0]);
		}

		[Fact]
		public void Execute_LDX_Long_Xr3()
		{
			BeforeEachTest();
			InstructionBuilder.BuildLongAtIar(OpCodes.LoadIndex, 3, 0x350, InsCpu);
			InsCpu.NextInstruction();
			InsCpu.ExecuteInstruction();
			Assert.Equal(0x350, InsCpu.Xr[3]);
		}

		[Fact]
		public void Execute_LDX_Long_Indirect_XR2()
		{
			BeforeEachTest();
			InstructionBuilder.BuildLongIndirectAtIar(OpCodes.LoadIndex, 2, 0x400, InsCpu);
			InsCpu.NextInstruction();
			InsCpu[0x400] = 0x1234;
			InsCpu.ExecuteInstruction();
			Assert.Equal(0x1234, InsCpu.Xr[2]);
		}

	    protected override void BuildAnInstruction()
	    {
			InstructionBuilder.BuildLongIndirectAtIar(OpCodes.LoadIndex, 2, 0x400, InsCpu);
		}

		protected override string OpName
		{
			get { return "LDX"; }
		}

		protected override OpCodes OpCode
		{
			get { return OpCodes.LoadIndex; }
		}

		[Fact]
		public override void NameAndOpcodeTest()
		{
			BeforeEachTest();
			CheckNameAndOpcode();
		}
	}
}