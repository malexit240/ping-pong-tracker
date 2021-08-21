using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace PingPong.Controls
{
    public class ClickableContentView : ContentView
    {
        public event EventHandler OnInvalidate;

        public ClickableContentView()
        {
            IsAnimated = true;
        }

        #region -- Protected properties --

        private State _currentState = State.Default;
        protected State CurrentState
        {
            get
            {
                return _currentState;
            }

            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    UpdateState();
                }
            }
        }

        #endregion

        #region -- Public properties --

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(ICommand));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CancelCommandProperty = BindableProperty.Create(
            propertyName: nameof(CancelCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(ICommand));

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly BindableProperty CancelCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CancelCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(object));

        public object CancelCommandParameter
        {
            get { return (object)GetValue(CancelCommandParameterProperty); }
            set { SetValue(CancelCommandParameterProperty, value); }
        }

        public static readonly BindableProperty MoveCommandProperty = BindableProperty.Create(
            propertyName: nameof(MoveCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(ICommand));

        public ICommand MoveCommand
        {
            get { return (ICommand)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }

        public static readonly BindableProperty MoveCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(MoveCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(object));

        public object MoveCommandParameter
        {
            get { return (object)GetValue(MoveCommandParameterProperty); }
            set { SetValue(MoveCommandParameterProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(object));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly BindableProperty SelectedScaleProperty = BindableProperty.Create(
            propertyName: nameof(SelectedScale),
            returnType: typeof(double),
            declaringType: typeof(ClickableContentView),
            defaultValue: 0.97);

        public double SelectedScale
        {
            get { return (double)GetValue(SelectedScaleProperty); }
            set { SetValue(SelectedScaleProperty, value); }
        }

        public static readonly BindableProperty IsAnimatedProperty = BindableProperty.Create(
            propertyName: nameof(IsAnimated),
            returnType: typeof(bool),
            declaringType: typeof(ClickableContentView),
            defaultValue: default(bool));

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        #endregion

        #region -- Public methods --

        public void Invalidate()
        {
            OnInvalidate?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region -- Protected methods --

        protected virtual void UpdateState()
        {
            if (Math.Abs(SelectedScale - 1.0f) > 0.001 && IsAnimated)
            {
                uint animationMiliseconds = 100;
                if (CurrentState == State.Selected)
                {
                    this.ScaleTo(SelectedScale, animationMiliseconds);
                }
                else
                {
                    if (Math.Abs(Scale - 1.0f) > 0.001)
                    {
                        this.ScaleTo(1.0f);
                    }
                }
            }
        }

        protected enum State
        {
            Default,
            Selected,
            Disabled,
        }

        #endregion

        #region -- Overrides --

        public bool TouchesBegan(IEnumerable<Point> points)
        {
            if (CurrentState != State.Disabled)
            {
                CurrentState = State.Selected;
            }

            return true;
        }

        public bool TouchesCancelled(IEnumerable<Point> points)
        {
            if (CurrentState != State.Disabled)
            {
                CurrentState = State.Default;
                OnCanceled();
            }

            return true;
        }

        public bool TouchesEnded(IEnumerable<Point> points)
        {
            if (CurrentState != State.Disabled)
            {
                CurrentState = State.Default;

                OnClicked();
            }

            return true;
        }

        public virtual bool TouchesMoved(IEnumerable<Point> points)
        {
            var point = points?.FirstOrDefault();

            if (point.Value.X < 0 || point.Value.Y < 0 || point.Value.X > this.Width || point.Value.Y > this.Height)
            {
                OnMoved();
            }

            return true;
        }

        #endregion

        #region -- Private helpers --

        private void OnClicked()
        {
            if (Command != null && IsEnabled && Command.CanExecute(CommandParameter))
            {
                Command?.Execute(CommandParameter);
            }
        }

        private void OnCanceled()
        {
            if (CancelCommand != null && IsEnabled && CancelCommand.CanExecute(CancelCommandParameter))
            {
                CancelCommand?.Execute(CancelCommandParameter);
            }
        }

        private void OnMoved()
        {
            if (MoveCommand != null && IsEnabled && MoveCommand.CanExecute(MoveCommandParameter))
            {
                MoveCommand?.Execute(MoveCommandParameter);
            }
        }

        #endregion
    }
}
