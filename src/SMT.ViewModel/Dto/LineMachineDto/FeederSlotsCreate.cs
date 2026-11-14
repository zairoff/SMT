namespace SMT.ViewModel.Dto.LineMachineDto
{
    // Adds slots From..To (inclusive) to a module; numbers that already exist are skipped
    // (reactivated if they were deactivated).
    public class FeederSlotsCreate
    {
        public int From { get; set; }

        public int To { get; set; }
    }
}
