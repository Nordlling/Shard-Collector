using DG.Tweening;

namespace App.Scripts.Modules.Dialogs.Interfaces
{
    public interface IDialogAnimator
    {
        Tween PlayShowDialog();
        Tween PlayHideDialog();
    }
}