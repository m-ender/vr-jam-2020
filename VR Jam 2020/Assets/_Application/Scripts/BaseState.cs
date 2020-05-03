namespace VRJam2020
{
    public enum BaseState
    {
        Free,
        FlyingToLeftHand,
        FlyingToRightHand,
        HeldInLeftHand,
        HeldInRightHand,
    }

    public static class BaseStateExtensionMethods
    {
        public static bool IsFlying(this BaseState baseState)
            => baseState == BaseState.FlyingToLeftHand || baseState == BaseState.FlyingToRightHand;

        public static bool IsBeingHeld(this BaseState baseState)
            => baseState == BaseState.HeldInLeftHand || baseState == BaseState.HeldInRightHand;
    }
}