   M S B A M L   `   `   `  ����   Act.Lib.ControllersLibrary2    ,Act.Lib.ControllersLibrary.GamepadController   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     
  J� Uid$  0��$	��300��$��30��$	��430��$	��430��$��11(� � 	Resources  �� MergedDictionaries �� Source��  �� $Q Kpack://application:,,,/Act.Lib.AppResources;component/Resources/Button.xaml?��� $V Ppack://application:,,,/Act.Lib.AppResources;component/Resources/RoundButton.xaml?��� $Q Kpack://application:,,,/Act.Lib.AppResources;component/Resources/Shared.xaml?��� $Q Kpack://application:,,,/Act.Lib.AppResources;component/Resources/Slider.xaml?�%    ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF464646�� � Offset$ 0Q�� $ 	#FFFFFFFF��$ 1Q�.��� -   $��GamepadGrid��$��10,15q�����  �� ImageSource$! Images/Gamepad/Logitech.png��.+��� -   $��Button1��$  1��$��28��$��28��   RoundButtonStyle#��[  $��230,-120,0,0q�
���   �$
��Bold"�$��16(�$��1���� -   $��Button2��$  2��$��28��$��28��#��[  $��292,-62,0,0q�
���   �$
��Bold"�$��16(�$��2���� -   $��Button3��$  3��$��28��$��28��#��[  $��
230,-2,0,0q�
���   �$
��Bold"�$��16(�$��3���� -   $��Button4��$  4��$��28��$��28��#��[  $��168,-62,0,0q�
���   �$
��Bold"�$��16(�$��4���� -   $��Button5��$  5��$��22��$��34��#��[  $��	100,5,0,0q�$
��Left=�$	��Top=�
���   �$
��Bold"�$��16(�$��5���� -   $��Button6��$  6��$��22��$��34��#��[  $��	0,5,100,0q�$��Right=�$	��Top=�
���   �$
��Bold"�$��16(�$��6���� -   $��Button7��$  7��$��22��$��34��#��[  $��30,0,0,0q�$
��Left=�$	��Top=�
���   �$
��Bold"�$��16(�$��7���� -	   $��Button8��$  8��$��22��$��34��#��[  $��0,0,30,0q�$��Right=�$	��Top=�
���   �$
��Bold"�$��16(�$��8���� -
   $��Button9��$  9��$��20��$��30��#��[  $��-62,-124,0,0q�
���   �$
��Bold"�$��16(�$��9���� -   $��Button10��$  10��$��20��$��30��#��[  $��66,-124,0,0q�
���   �$
��Bold"�$��16(�$��10���� -   $��SliderX��$��1��$��56��$��23��$��Center=�$��-225,-60,0,0q�$��Center=�	 � Value$	 0Q�
 � Minimum$
 -1000Q� � Maximum$
 1000Q��� -   $��SliderY��$��1�� �� Orientation$ Vertical=�$��23��$��56��$��Center=�$��82,-60,305,0q�$��Center=�$	 0Q�$
 -1000Q�$
 1000Q� �� IsDirectionReversed . �� -   $��XaxisValueLabel��$��1��$��45��$��24��$��X axis: ��$��Center=�$��-110,230,0,0q�$
��Bold"��� -   $��
XaxisValue��$��1��$��40��$��24��$�� ��$��-40,230,0,0q��� -   $��YaxisValueLabel��$��1��$��45��$��24��$��Y axis: ��$��Center=�$��
60,230,0,0q�$
��Bold"��� -   $��
YaxisValue��$��1��$��40��$��24��$�� ��$��140,230,0,0q��� -   $��ButtonsLabel��$��1��$��Button(s): ��$��Center=�$��
-60,0,0,60q�$��Bottom=�$
��Bold"��� -   $��ButtonListLabel��$��1��$�� ��$��center=�$��5,0,0,60q�$��Bottom=��� -   $��
JoyIDLabel��$��1��$��
Controller��$��28��$��Center=�$
��Bold"�
���  ���� -   $��Button1Label��$��1��$��Button 1��$	��100��
�������$��
270,40,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button2Label��$��1��$��Button 2��$��60��
�������$��
360,92,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button3Label��$��1��$��Button 3��$	��100��
�������$��270,144,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button4Label��$��1��$��Button 4��$��60��
�������$��
216,92,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button5Label��$��1��$��Button 5��$��60��
�������$��	90,20,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button6Label��$��1��$��Button 6��$��60��
�������$��	0,20,90,0q�$��Right=�$	��Top=�$��Center=��� -   $��Button7Label��$��1��$��Button 7��$��60��
�������$��	18,15,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button8Label��$��1��$��Button 8��$��60��
�������$��	0,15,18,0q�$��Right=�$	��Top=�$��Center=��� -   $��Button9Label��$��1��$��Button 9��$��64��
�������$��
140,42,0,0q�$
��Left=�$	��Top=�$��Center=��� -   $��Button10Label��$��1��$��	Button 10��$��64��
�������$��
0,42,140,0q�$��Right=�$	��Top=�$��Center=��� -   $��XAxisLabelLeft��$��X Axis L��$��60��
�������$��0,92,0,0q�$
��Left=�$	��Top=�$��Right=��� -    $��YAxisLabelDown��$��Y Axis D��$	��100��
�������$��
42,144,0,0q�$
��Left=�$	��Top=�$��Center=��� -!   $��XAxisLabelRight��$��X Axis R��$��60��
�������$��
120,92,0,0q�$
��Left=�$	��Top=�$
��Left=��� -"   $��YAxisLabelUp��$��Y Axis U��$	��100��
�������$��	42,54,0,0q�$
��Left=�$	��Top=�$��Center=�