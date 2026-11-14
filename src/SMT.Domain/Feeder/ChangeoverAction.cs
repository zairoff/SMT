namespace SMT.Domain.Feeder
{
    public enum ChangeoverAction
    {
        // Same component stays in the same slot - still re-verified, the recorded slot state may be stale.
        Keep = 0,
        // Slot was empty (or unknown) and gets a component.
        Load = 1,
        // The component is already on the line in another slot and moves here.
        Move = 2,
        // Slot holds a component the target setup doesn't use - it has to be emptied.
        Remove = 3,
        // Slot holds a different component than the target setup wants.
        Replace = 4,
    }
}
