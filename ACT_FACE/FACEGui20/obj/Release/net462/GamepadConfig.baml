   M S B A M L   `   `   `  ���� M  IAct.Face.FACEGui20, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Act.Face.FACEGui20Act.Face.FACEGui20  &     Act.Face.FACEGui20.GamepadConfig   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:Act.Face.FACEGui20       � Title$  
MainWindow��$��526.125��$��888.8�� � 
ResizeMode$ NoResize=� � SizeToContent$ Width=� � 	Resources    ComboBoxToggleButton �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius F�Name	 �� Triggers
 P� Property P� Value �� 
TargetName �� Property �� Value  ComboBoxTextBox �� 
TargetType �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight  K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation!   DirectionalNavigation" P� 
SourceName# �� Triggers$ �� Padding��%�	  &	        &	 �    '�� �    7   7  '��     �� " d�.R��  �� �� $��20 �.+��� $ Border��$��2��$ 0�����	#FF3F3F3F���	#FF97A0A5$��1q��� $��0��$ 0��$��1q����	#FF3F3F3F���	#FF97A0A5$��0,0,1,0q�F� $ Arrow��$��1��
�������$��Center=�$��Center=�=����        �    �� � 	= ��[ � z �� � z     � 	=  	= 		 P� 
 �@J�IsMouseOver . . ��� $ Border�� � �� �#808080P� 
 �@d�	IsChecked$
 trueY�. ��� $ Border�� � �� �#E0E0E0P� 
 � {� .  . ��� $ Border�� � �� �#EEEEEE�� $ Border�� � �� �#AAAAAA��  � �� �#888888�� $ Arrow�� � �� �#888888�� " ��.R��� $ PART_ContentHost��|�.  #��z���� " ��.���  �@J�SnapsToDevicePixels . ��  �@�OverridesDefaultStyle . ��  � ��$
 Auto=���  � ��$
 Auto=���  � �� . ��  � ��$	 120����  � ��$ 20����  � ��
 �������  � �� �� " ��.R�� .+�d� $��ToggleButton��8��  $��2��|�.  $ Press=� �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+�� $��ContentSite�� .  #��z #��z #��z��$��3,3,23,3q�$��Center=�$
��Left=��� $ PART_EditableTextBox��8�� $
��Left=�$��Center=�$��3,3,23,3q�|�. ���	#FF3F3F3F
��� � �$y�Hidden=�# z ��Z� � $��Popup��$��Bottom=�#��z  . |�.  $��Slide=�.��� $��DropDown�� . #��z #��z .+��� $ DropDownBorder�����	#FF3F3F3F$��1q����#888888�� $��4,6,4,6q� . .����   . $! 	Contained=�	 P� 
 �@��HasItems .  . ��� $ DropDownBorder�� � ��$ 95��P� 
 � {� .  . ���  � �� �#888888P� 
 �@��
IsGrouping . . ���  � �� .  P� $" Popup��
 �@�AllowsTransparency . . ��� $ DropDownBorder�� �@��CornerRadius$ 0���� $ DropDownBorder�� � ��$ 0,2,0,0q�P� 
 �@��
IsEditable . . ���  � �� .  �� $ PART_EditableTextBox�� � y�$ Visible=��� $ ContentSite�� � y�$ Hidden=�# �� " ��.���  �@J�SnapsToDevicePixels . ��  � ��
 �������  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $��Border��$$ 2q� . ._��� 	 P� 
 �@��IsHighlighted . . ��� $ Border�� � �� �	#FF4F4F4FP� 
 � {� .  . ���  � �� �#888888.��� .+��� $	��500��$��0,0,0,0q�$��888.8��.+��� -   % ��Name$% 
cb_new_jpg��$��0,0,0,0q�$��/Images/cb_final.jpg��$	��Top=�$	��500��$	��889��$��UniformToFill=�& J� OpacityMask& Z� �� -   ' ��Name$' cbLeftShoulder��$	��120��( �� Left$
( 76.8��
) �� Top$	) 108��$��10(�$
��Auto���� -   $' cbLeftTrigger��$	��120��$( 77��$	) 152��$��10(�$
��Auto���� -   $' cbLeftThumb��$	��120��$( 77��$	) 244��$��10(�$
��Auto���� -   $' cbDPadUp��$	��120��$( 77��$	) 287��$��10(�$
��Auto���� -   $' cbDPadRight��$	��120��$( 77��$	) 307��$��10(�$
��Auto���� -   $' 
cbDPadLeft��$	��120��$( 77��$	) 327��$��10(�$
��Auto���� -   $' 
cbDPadDown��$	��120��$( 77��$	) 347��$��10(�$
��Auto���� -	   $' cbRightShoulder��$	��120��$	( 690��$	) 108��$��10(�$
��Auto���� -
   $' cbRightTrigger��$	��120��$	( 690��$	) 152��$��10(�$
��Auto���� -   $	' cbX��$	��120��$	( 690��$	) 184��$��10(�$
��Auto���� -   $	' cbY��$	��120��$	( 690��$	) 211��$��10(�$
��Auto���� -   $	' cbB��$	��120��$	( 690��$	) 238��$��10(�$
��Auto���� -   $	' cbA��$	��120��$	( 690��$	) 266��$��10(�$
��Auto���� -   $' cbRightThumb��$	��120��$	( 690��$	) 313��$��10(�$
��Auto���� -   * ��Name$* btnSave��$
��Save��$	( 754��$	) 447��$��94��$��31�����	#FF3F3F3F���	#FF97A0A5���#888888