using System;
using System.Threading;
using App.Scripts.Modules.Pool.Abstract;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Modules.Dialogs.Interfaces
{
    public abstract class PoolableDialog : MonoPoolableItem
    {
        [SerializeField] private IDialogAnimator dialogAnimator;
        
        public event Action OnDialogClose;
        public event Action OnDialogStartClose;
        public event Action<PoolableDialog, Action> OnCloseClick;

        private bool _isOpened;

        public override void OnInitializeItem()
        {
            OnDialogInitialize();
        }

        public override void OnSetupItem()
        {
            OnDialogSetup();
        }

        public override void OnResetItem()
        {
            OnDialogReset();
        }

        public Tween ShowDialog(Action onComplete)
        {
            _isOpened = true;

            var tween = DOTween.Sequence()
                .AppendCallback(OnDialogStartShow)
                .Append(dialogAnimator?.PlayShowDialog())
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    OnDialogFinishShow();
                });

            return tween;
        }

        public Tween CloseDialog(Action onComplete)
        {
            var tween = DOTween.Sequence()
                .AppendCallback(() => OnDialogStartClose?.Invoke())
                .Append(dialogAnimator?.PlayHideDialog())
                .OnComplete(() =>
                {
                    _isOpened = false;
                    Remove();
                    onComplete?.Invoke();
                    OnDialogClose?.Invoke();
                    OnDialogClose = null;
                });

            return tween;
        }

        public virtual void Close(Action onComplete = null)
        {
            OnCloseClick?.Invoke(this, onComplete);
        }

        public UniTask WaitForClose(CancellationToken cancellationToken = default)
        {
            return UniTask.WaitWhile(() => _isOpened, cancellationToken: cancellationToken);
        }

        protected virtual void OnDialogInitialize() { }
        protected virtual void OnDialogSetup() { }
        protected virtual void OnDialogStartShow() { }
        protected virtual void OnDialogFinishShow() { }
        protected virtual void OnDialogReset() { }
    }
}