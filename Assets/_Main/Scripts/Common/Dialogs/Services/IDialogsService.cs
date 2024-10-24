using System;
using DG.Tweening;
namespace App.Scripts.Modules.Dialogs.Interfaces
{
	public interface IDialogsService
	{
		bool HasDialog<TDialog>() where TDialog : PoolableDialog;
		bool HasAnyDialog();
		TDialog GetDialog<TDialog>() where TDialog : PoolableDialog;
		Tween ShowDialog(PoolableDialog dialog, Action onComplete =null);
		Tween CloseDialog<TDialog>(Action onComplete = null) where TDialog : PoolableDialog;
		event Action<PoolableDialog> OnDialogStartClose;
	}
}