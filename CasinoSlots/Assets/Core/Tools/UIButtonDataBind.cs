using AxGrid.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders{
	
	/// <summary>
	/// Бинд кнопки в модель
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class UIButtonDataBind : Binder
	{
		private Button button;
		/// <summary>
		/// Имя кнопки (если пустое берется из имени объекта)
		/// </summary>
		public string buttonName = "";

		public string enableField = "";

		/// <summary>
		/// Включена по умолчанию
		/// </summary>
		public bool defaultEnable = true;

		/// <summary>
		/// Поле из модели где взять настройку клавиатуры
		/// </summary>
		public string keyField = "";
		
		/// <summary>
		/// Кнопка клавиатуры (заполнится из модели если там есть)
		/// </summary>
		public string key = "";

		/// <summary>
		/// Срабатывать на нажатие
		/// </summary>
		public bool onKeyPress = false;

		/// <summary>
		/// Отправляет события во вспомогательную UI fsm
		/// </summary>
		public bool isFsmUI = false;
		
		private bool down = false;
		private float downTime = 0.0f;
		private bool cancel = false;

		private EventTrigger et;
		
		[OnAwake]
		public void awake()
		{
			button = GetComponent<Button>();
			if (string.IsNullOrEmpty(buttonName))
				buttonName = name;
			
			enableField = enableField == "" ? $"Btn{buttonName}Enable" : enableField;
			if (!onKeyPress)
				button.onClick.AddListener(OnClick);
			else
			{
				et = gameObject.AddComponent<EventTrigger>();
				var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerDown};
				entry.callback.AddListener(OnClick);
				et.triggers.Add(entry);
			}
		}

		[OnStart]
		public void start()
		{
			Model.EventManager.AddAction($"On{enableField}Changed", OnItemEnable);
			if (keyField == "")
				keyField = $"{name}Key";
			if (keyField != "")
			{
				key = Model.GetString(keyField, key);
				Model.EventManager.AddAction($"On{keyField}Changed", OnKeyChanged);
			}
			OnItemEnable();
			
		}

		public void OnKeyChanged()
		{
			key = Model.GetString(keyField);
		}
		
		public void CancelClick() {
			cancel = !onKeyPress;
		}
		
		public void OnItemEnable()
		{
			if (button.interactable != Model.GetBool(enableField, defaultEnable))
				button.interactable = Model.GetBool(enableField, defaultEnable);
		}

		[OnDestroy]
		public void onDestroy()
		{
			button.onClick.RemoveAllListeners();
			if (et != null)
			{
				et.triggers.ForEach(t => t.callback.RemoveAllListeners());
				et.triggers.Clear();
			}
			Model.EventManager.RemoveAction($"On{enableField}Changed", OnItemEnable);
			Model.EventManager.RemoveAction($"On{keyField}Changed", OnKeyChanged);
		}

		private void OnClick(BaseEventData bd)
		{
			Log.Debug("CLICK!");
			if (!cancel)
				OnClick();
			cancel = false;
		}
		
		public void OnClick()
		{
			if (!button.interactable || !isActiveAndEnabled)
				return;

			if (!cancel)
			{
				Model?.EventManager.Invoke("SoundPlay", "Click");
				
				Settings.Fsm?.Invoke("OnBtn", buttonName);
				
				Model?.EventManager.Invoke($"On{buttonName}Click");
			}
			cancel = false;
		}

		
		[OnUpdate]
		protected void update()
		{
			if (!button.interactable || key == "")
				return;
					
			if (onKeyPress && !down && Input.GetKeyDown(key)) {
				if (onKeyPress)
					OnClick();

				if (!down)
				{
					down = true;
					downTime = 0;
				}
			}
			if (!onKeyPress && Input.GetKeyUp(key)) Log.Info($"Key:{key} / D:{down} / C:{cancel}");
			if (!onKeyPress && Input.GetKeyUp(key)) {
					OnClick();
				down = true;
			}
			
			if (Input.GetKeyUp(key)) {
				cancel = false;
				down = false;
			}

			if (down)
				downTime += Time.deltaTime;
			if (downTime >= 2f)
			{
				down = false;
				cancel = false;
			}
		}
	}
}