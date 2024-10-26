using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.Common.InputSystem;
using App.Scripts.Modules.Dialogs.Interfaces;
using App.Scripts.Modules.Pool.Interfaces.Pool;
using DG.Tweening;
using UnityEngine;
namespace App.Scripts.Modules.Dialogs.Services
{
	public class DialogsService : IDialogsService
	{
		public event Action<PoolableDialog> OnDialogStartClose;

		private readonly List<PoolableDialog> _activeDialogs = new ();

		private readonly ITypePool<PoolableDialog> _pool;
		private readonly Transform _dialogsContainer;
		private readonly IInputService _inputService;

		public DialogsService(ITypePool<PoolableDialog> pool, Transform dialogsContainer, IInputService inputService)
		{
			_pool = pool;
			_dialogsContainer = dialogsContainer;
			_inputService = inputService;
		}

		public T GetDialog<T>() where T : PoolableDialog
		{
			T dialog = _pool.Get<T>();
			dialog.transform.SetParent(_dialogsContainer, false);
			
			_activeDialogs.Add(dialog);
			
			dialog.OnCloseClick += OnDialogClose;

			return dialog;
		}

		public bool HasDialog<TDialog>() where TDialog : PoolableDialog
		{
			return _activeDialogs.Any(el => el.GetType() == typeof(TDialog));
		}

		public bool HasAnyDialog()
		{
			return _activeDialogs.Count > 0;
		}

		public Tween ShowDialog(PoolableDialog dialog, Action onComplete = null)
		{
			_inputService.DisableInput();
			return dialog.ShowDialog(() =>
			{
				onComplete?.Invoke();
			});
		}
		
		public Tween CloseDialog<TDialog>(Action onComplete = null) where TDialog : PoolableDialog
		{
			var type = typeof(TDialog);
		
			if (GetActiveDialog(type) != null)
			{
				return CloseDialog(type, onComplete);
			}
		
			return DOTween.Sequence();
		}
		
		private Tween CloseDialog(Type type, Action onComplete = null)
		{
			var dialog = GetActiveDialog(type);
		
			if (dialog == null)
			{
				return DOTween.Sequence();
			}
			
		
			dialog.OnCloseClick -= OnDialogClose;
		
			OnDialogStartClose?.Invoke(dialog);
		
			return dialog.CloseDialog(() =>
			{
				onComplete?.Invoke();
				_inputService.EnableInput();
			});
		}
		
		private void OnDialogClose(PoolableDialog dialog, Action onComplete)
		{
			CloseDialog(dialog.GetType(), onComplete);
		}
		
		private PoolableDialog GetActiveDialog(Type type)
		{
			return _activeDialogs.Find(el => el.GetType() == type);
		}
		
	}
}