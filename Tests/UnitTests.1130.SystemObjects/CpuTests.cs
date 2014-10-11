﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using S1130.SystemObjects;
using FakeItEasy;
using S1130.SystemObjects.Instructions;

namespace UnitTests.S1130.SystemObjects
{
    [TestClass]
    public class CpuTests
    {
        private Cpu _cpu;
        private const ushort IarDefault = 0x100;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _cpu = new Cpu { Iar = 0x100 };
        }

        [TestMethod]
        public void AccProperty()
        {
            _cpu.Acc = 0x1234;
            Assert.AreEqual(0x1234, _cpu.Acc);
        }

        [TestMethod]
        public void ExtProperty()
        {
            _cpu.Ext = 0x1234;
			Assert.AreEqual(0x1234, _cpu.Ext);
        }

        [TestMethod]
        public void IarProperty()
        {
            _cpu.Iar = 0x1234;
			Assert.AreEqual(0x1234, _cpu.Iar);
        }

        [TestMethod]
        public void AtIarProperty()
        {
            _cpu.AtIar = 0x1234;
            Assert.AreEqual(0x1234, _cpu[IarDefault]);
            _cpu[IarDefault] = 0x3245;
            Assert.AreEqual(0x3245, _cpu.AtIar);
        }

		[TestMethod]
		public void IndexProperty()
		{
			_cpu[0x100] = 0x1234;
			Assert.AreEqual(0x1234, _cpu.Memory[0x100]);
		}

		[TestMethod]
		public void CarryProperty()
		{
			_cpu.Carry = true;
			Assert.IsTrue(_cpu.Carry);
			_cpu.Carry = false;
			Assert.IsFalse(_cpu.Carry);
		}

		[TestMethod]
		public void OverflowProperty()
		{
			_cpu.Overflow = true;
			Assert.IsTrue(_cpu.Overflow);
			_cpu.Overflow = false;
			Assert.IsFalse(_cpu.Overflow);
		}

	    [TestMethod]
	    public void InvalidOpCode()
	    {
		    _cpu.AtIar = 0x0000;
			_cpu.NextInstruction();
			_cpu.ExecuteInstruction();
			Assert.AreEqual(0x101, _cpu.Iar);
			Assert.IsTrue(_cpu.Wait);
	    }

		[TestMethod]
		public void NextInstruction_ShortLoadInstructionTest()
		{
			_cpu.Memory[0x100] = InstructionBuilder.BuildShort(OpCodes.Load, 2, 0x72);
			_cpu.NextInstruction();
			Assert.AreEqual(0x101, _cpu.Iar);
			Assert.AreEqual((int)OpCodes.Load, _cpu.Opcode);
			Assert.AreEqual(false, _cpu.FormatLong);
			Assert.AreEqual(2, _cpu.Tag);
			Assert.AreEqual(0x72, _cpu.Displacement);
			Assert.AreEqual("LD", _cpu.CurrentInstruction.OpName);
		}

		[TestMethod]
		public void NextInstruction_LongLoadInstructionIndirectWithXR3Test()
		{
			InstructionBuilder.BuildLongIndirectAtIar(OpCodes.Load, 3, 0x72, _cpu);
			_cpu.NextInstruction();
			Assert.AreEqual(0x102, _cpu.Iar);
			Assert.AreEqual((int)OpCodes.Load, _cpu.Opcode);
			Assert.AreEqual(true, _cpu.FormatLong);
			Assert.AreEqual(true, _cpu.IndirectAddress);
			Assert.AreEqual(3, _cpu.Tag);
			Assert.AreEqual(0x72, _cpu.Displacement);
		}

		[TestMethod]
		public void IndexerTest()
		{
			_cpu[0x1000] = 0xbfbf;
			Assert.AreEqual(0xbfbf, _cpu.Memory[0x1000]);
			_cpu.Memory[0x2000] = 0xfbfb;
			Assert.AreEqual(0xfbfb, _cpu[0x2000]);
		}
	}
}
