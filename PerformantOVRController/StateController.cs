using System.Collections.Generic;
using PerformantOVRController.Hands;
using PerformantOVRController.Hands.HandStates;
using PerformantOVRController.Locomotion.Walker;
using PerformantOVRController.Locomotion.Walker.Interfaces;
using PerformantOVRController.Locomotion.Walker.WalkingStates;

namespace PerformantOVRController
{
    public class StateController
    {
        #region walker
        private readonly Dictionary<WalkStates, LocomotionState> _walkingStates = new();
        private readonly StateWalker _player;
        
        public StateController(StateWalker player)
        {
            _player = player;
            _walkingStates.Add(WalkStates.Idle, new WalkStateIdle(player));
            _walkingStates.Add(WalkStates.Jump, new WalkStateJumping(player));
            _walkingStates.Add(WalkStates.Sprint, new WalkStateSprinting(player));
            _walkingStates.Add(WalkStates.Walk, new WalkStateWalking(player));
            _player.currentState = _walkingStates[WalkStates.Idle];
        }

        
        public void ChangeState(WalkStates state)
        {
            if (_player.currentState.walkState == state) return;
            _player.currentState?.ExitState();
            _player.currentState = _walkingStates[state];
            _player.currentState.EnterState();
        }
        
        #endregion

        #region hand
        private readonly Dictionary<HandState, HandStateClass> _handStates = new();
        private readonly Hand _hand;
        
        public StateController(Hand hand)
        {
            _hand = hand;
            _handStates.Add(HandState.Closed, new HandClosed());
            _handStates.Add(HandState.Grabbing, new HandGrabbing());
            _handStates.Add(HandState.Grabbed, new HandGrabbed());
            _handStates.Add(HandState.Open, new HandOpen());
            _handStates.Add(HandState.Teleport, new HandTeleport());
            _hand.currentState = _handStates[HandState.Open];
        }

        public void ChangeState(HandState state)
        {
            if (_hand.currentState.handState == state) return;
            _hand.currentState?.ExitState();
            _hand.currentState = _handStates[state];
            _hand.currentState.EnterState();
        }
        #endregion
    }

}