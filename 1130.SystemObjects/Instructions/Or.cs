namespace S1130.SystemObjects.Instructions
{
	public class Or : InstructionBase, IInstruction
	{
		public OpCodes OpCode { get { return OpCodes.Or; }  }
		public string OpName { get { return "OR";  } }

		public void Execute(ISystemState state)
		{	
			state.Acc |= state[GetEffectiveAddress(state)];
		}
	}
}