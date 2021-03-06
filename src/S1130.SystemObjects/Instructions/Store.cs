namespace S1130.SystemObjects.Instructions
{
    public class Store : InstructionBase, IInstruction
    {
        public OpCodes OpCode { get {return OpCodes.Store; } }
        public string OpName { get { return "STO"; } }

        public void Execute(ICpu cpu)
        {
            cpu[GetEffectiveAddress(cpu)] = cpu.Acc;
        }
    }
}