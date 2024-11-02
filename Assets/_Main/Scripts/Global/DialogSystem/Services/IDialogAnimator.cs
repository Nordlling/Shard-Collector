using DG.Tweening;

namespace _Main.Scripts.Global.DialogSystem.Services
{
    public interface IDialogAnimator
    {
        Tween PlayShowDialog();
        Tween PlayHideDialog();
    }
}