﻿
using UnityEngine;

namespace TaoTie
{
    /// <summary>
    /// 玩家的输入
    /// </summary>
    public class LocalInputController : Component, IComponent, IUpdate
    {
        private MoveComponent moveComponent => parent.GetComponent<MoveComponent>();
        private NumericComponent numericComponent => parent.GetComponent<NumericComponent>();

        private FsmComponent fsm => parent.GetComponent<FsmComponent>();
        private bool canMove = true;
        private bool canTurn = true;
        
        #region IComponent

        public void Init()
        {
            canMove = fsm.DefaultFsm.CurrentState.CanMove;
            canTurn = fsm.DefaultFsm.CurrentState.CanTurn;
            Messager.Instance.AddListener<bool>(Id, MessageId.SetCanMove, SetCanMove);
            Messager.Instance.AddListener<bool>(Id, MessageId.SetCanTurn, SetCanTurn);
        }

        public void Destroy()
        {
            Messager.Instance.RemoveListener<bool>(Id, MessageId.SetCanMove, SetCanMove);
            Messager.Instance.RemoveListener<bool>(Id, MessageId.SetCanTurn, SetCanTurn);
        }

        public void Update()
        {
            //移动
            Vector3 direction = Vector3.zero;
            if (InputManager.Instance.GetKey(GameKeyCode.MoveForward))
            {
                direction += Vector3.forward;
            }
            if (InputManager.Instance.GetKey(GameKeyCode.MoveBack))
            {
                direction += Vector3.back;
            }
            if (InputManager.Instance.GetKey(GameKeyCode.MoveLeft))
            {
                direction += Vector3.left;
            }
            if (InputManager.Instance.GetKey(GameKeyCode.MoveRight))
            {
                direction += Vector3.right;
            }

            if (direction == Vector3.zero)
            {
                if (InputAxisBind.AxisBind.TryGetValue(AxisType.Horizontal, out float x)
                    && InputAxisBind.AxisBind.TryGetValue(AxisType.Vertical, out float y))
                {
                    direction += Vector3.forward * y;
                    direction += Vector3.right * x;
                }
            }
            
            TryJump();
            this.TryMove(direction);
        }

        #endregion

        public void TryMove(Vector3 direction, MotionFlag mFlag = MotionFlag.Run, MotionDirection mDirection = MotionDirection.Forward)
        {
            if (moveComponent == null) return;
            if (!canTurn)
            {
                if (mDirection == MotionDirection.Left || mDirection == MotionDirection.Right)
                {
                    mDirection = MotionDirection.Forward;
                }
                direction = new Vector3(0, 0, direction.z);
            }
            if (direction == Vector3.zero)
            {
                fsm.SetData(FSMConst.MotionFlag, 0);
                fsm.SetData(FSMConst.MotionDirection, 0);
            }
            else
            {
                fsm.SetData(FSMConst.MotionFlag, (int)mFlag);
                fsm.SetData(FSMConst.MotionDirection, (int)mDirection);
            }

            if (moveComponent?.CharacterInput != null)
            {
                moveComponent.CharacterInput.MotionDirection = mDirection;
                if (canMove)
                    moveComponent.CharacterInput.Direction = direction.normalized;
                else
                    moveComponent.CharacterInput.Direction = Vector3.zero;
                if (CameraManager.Instance.MainCamera() != null && 
                    CameraManager.Instance.CurrentCameraState.Data.AvatarFaceDirection)
                {
                    moveComponent.CharacterInput.FaceDirection = CameraManager.Instance.MainCamera().transform.forward;
                }
                else
                {
                    moveComponent.CharacterInput.FaceDirection = Vector3.forward;
                }
                //因为是动画驱动移动，所以这里速度指的是动画速度缩放比例
                moveComponent.CharacterInput.SpeedScale = numericComponent.GetAsFloat(NumericType.Speed);
            }
        }
        
        private void SetCanMove(bool canMove)
        {
            if(IsDispose) return;
            this.canMove = canMove;
        }

        private void SetCanTurn(bool canTurn)
        {
            if(IsDispose) return;
            this.canTurn = canTurn;
        }

        public void TryJump()
        {
            if (InputManager.Instance.GetKey(GameKeyCode.Jump) && fsm.DefaultFsm.CurrentState.CanJump)
            {
                fsm.SetData(FSMConst.Jump, true);
            }
        }
    }
}