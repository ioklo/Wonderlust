using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// Copy From Microsoft.Xaml.Behaviors.Input.KeyTrigger
namespace Wonderlust.WPF.Miscs
{
    public class KeyTriggerEx : EventTriggerBase<UIElement>
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(Key), typeof(KeyTriggerEx));

        public static readonly DependencyProperty ModifiersProperty = DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(KeyTriggerEx));

        public static readonly DependencyProperty ActiveOnFocusProperty = DependencyProperty.Register("ActiveOnFocus", typeof(bool), typeof(KeyTriggerEx));

        public static readonly DependencyProperty FiredOnProperty = DependencyProperty.Register("FiredOn", typeof(KeyTriggerFiredOn), typeof(KeyTriggerEx));

        private UIElement? targetElement;

        /// <summary>
        /// The key that must be pressed for the trigger to fire.
        /// </summary>
        public Key Key
        {
            get { return (Key)this.GetValue(KeyTriggerEx.KeyProperty); }
            set { this.SetValue(KeyTriggerEx.KeyProperty, value); }
        }

        /// <summary>
        /// The modifiers that must be active for the trigger to fire (the default is no modifiers pressed).
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)this.GetValue(KeyTriggerEx.ModifiersProperty); }
            set { this.SetValue(KeyTriggerEx.ModifiersProperty, value); }
        }

        /// <summary>
        /// If true, the Trigger only listens to its trigger Source object, which means that element must have focus for the trigger to fire.
        /// If false, the Trigger listens at the root, so any unhandled KeyDown/Up messages will be caught.
        /// </summary>
        public bool ActiveOnFocus
        {
            get { return (bool)this.GetValue(KeyTriggerEx.ActiveOnFocusProperty); }
            set { this.SetValue(KeyTriggerEx.ActiveOnFocusProperty, value); }
        }

        /// <summary>
        /// Determines whether or not to listen to the KeyDown or KeyUp event.
        /// </summary>
        public KeyTriggerFiredOn FiredOn
        {
            get { return (KeyTriggerFiredOn)this.GetValue(KeyTriggerEx.FiredOnProperty); }
            set { this.SetValue(KeyTriggerEx.FiredOnProperty, value); }
        }

        protected override string GetEventName()
        {
            return "Loaded";
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            // NOTICE: alt + c의 경우 Key = Key.System, SystemKey = Key.C가 된다
            if (e.Key == Key.System)
            {
                if (e.SystemKey == this.Key &&
                    this.Modifiers == GetActualModifiers(e.Key, Keyboard.Modifiers))
                {   
                    this.InvokeActions(e);
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == this.Key &&
                    this.Modifiers == GetActualModifiers(e.Key, Keyboard.Modifiers))
                {
                    this.InvokeActions(e);
                    e.Handled = true;
                }
            }
        }

        private static ModifierKeys GetActualModifiers(Key key, ModifierKeys modifiers)
        {
            if (key == Key.LeftCtrl || key == Key.RightCtrl)
            {
                modifiers |= ModifierKeys.Control;
            }
            else if (key == Key.LeftAlt || key == Key.RightAlt || key == Key.System)
            {
                modifiers |= ModifierKeys.Alt;
            }
            else if (key == Key.LeftShift || key == Key.RightShift)
            {
                modifiers |= ModifierKeys.Shift;
            }
            return modifiers;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            // Listen to keyboard events.
            if (this.ActiveOnFocus)
            {
                this.targetElement = this.Source;
            }
            else
            {
                this.targetElement = KeyTriggerEx.GetRoot(this.Source);
            }

            Debug.Assert(this.targetElement != null);

            if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
            {
                this.targetElement.KeyDown += this.OnKeyPress;
            }
            else
            {
                this.targetElement.KeyUp += this.OnKeyPress;
            }
        }

        protected override void OnDetaching()
        {
            if (this.targetElement != null)
            {
                if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
                {
                    this.targetElement.KeyDown -= this.OnKeyPress;
                }
                else
                {
                    this.targetElement.KeyUp -= this.OnKeyPress;
                }
            }

            base.OnDetaching();
        }

        private static UIElement? GetRoot(DependencyObject current)
        {
            UIElement? last = null;

            while (current != null)
            {
                last = current as UIElement;
                current = VisualTreeHelper.GetParent(current);
            }

            return last;
        }
    }
}
