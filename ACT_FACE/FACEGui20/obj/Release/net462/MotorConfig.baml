   M S B A M L   `   `   `  ���� U  QAct.Lib.ControllersLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibraryAct.Lib.ControllersLibrary   Act.Face.FACEGui20$   Act.Face.FACEGui20.MotorConfig   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^localLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibrary       � Title$$  Servo Motor Configuration Tool�� � 
ResizeMode$ NoResize=�$	��550��$	��500�� � 	Resources  �� MergedDictionaries �� Source��  �� $Q Kpack://application:,,,/Act.Lib.AppResources;component/Resources/Shared.xaml?��� $R Lpack://application:,,,/Act.Lib.AppResources;component/Resources/Slider2.xaml?�%    .��� ����  �� ImageSource$ Images/sfondo.jpg�� � RowDefinitions �� $��* ��� $��24 �.+�� -    �Name$ LayoutRootOld��$��0��$
��0,10q�.+��� -      ToolBarButtonBaseStyle#��[   � ToolTip$
 Save��$��2,0,0,0q�$��36��$
��Left=�$	��Top=�.���� $*��$/Images/ToolbarMenu/File_save-32.png���� -   #��[  $ Save As��$��0,40,0,0q�$��36��$
��Left=�$	��Top=�.���� $-��'/Images/ToolbarMenu/File_save_as-32.png��� -   $ 
ConfigGrid��$��0��$��3��$��Center=� �� $��34 ��� $	��100 ��� $��34 ��� $��34 ��� $��64 ��� $��* ��� $��54 �.+��� $��0��$��Servomotor to configure: ��$
��14px(����	#FF999999$��Trebuchet MS)�$
��Bold"�$��28��$
��Left=�$��0,10,0,0q��� -   	 ��Name$	 lblServoMotor��$��0��$�� ��$
��14px(�
�������$��Trebuchet MS)�$
��Bold"�$��28��$
��Left=�$��	180,0,0,0q��� -   
 ��Name$
 imgSevo��$��1��$��/Images/servo.png��$	��100���� $��2��$��Servo Controller:��$
��14px(����	#FF999999$��Trebuchet MS)�$
��Bold"�$��28��$��Center=�$	��Top=�$��	61,6,79,0q��� -    ��Name$ ServoControllerPortCombo��$��3��$��
-5,6,177,7q� �� MaxDropDownHeight$	 267��$��Center=�$��21��$��22��{�.  �� -   $ ServoControllerCombo��$��3��$��
116,6,74,7q�$	 267��$��Center=�$��21��$	��112��{�.  �� -	    ��Name$ SetSC��$��3��  ButtonStyle#��[ $��
235,3,29,3q� �� 	IsDefault . $��38��$��Center=�$��Center=�{�.  .���� -
   $	 SerialButtonLabel��$	��Set��$��Center=��� $��4��$#��Set minimum e maximum values:��$
��14px(����	#FF999999$��Trebuchet MS)�$
��Bold"�$
��26.9��$��Center=�$��Bottom=�$��	18,0,17,0q��� -   $��Border��$��5��$	��0,5q� �� Padding$ 2q� J� SnapsToDevicePixels . $��1q� �� CornerRadius$ 2��._�]� -    ]�Name$ ManualPanel��$	��110��{�.  .+�� $��0,0,0,10q�$	��Top=���Z�  � ColumnDefinitions �� $��58 ��� $��* ��� $��58 �.+��� -   $ SetMinButton��#��[ $��0��$��0,3,0,3q� . $��58��$��Center=�$��Center=�.���� -   $	 MinButtonLabel��$��Set Min��$��Center=��� -    ��Name$ PositionBox��$��1��$�� ��$��Stretch=� �� TextWrapping$
 Wrap=�$��Center=�$��	45,0,45,0q� �� TextAlignment$ Center=�{�.  �� -   $ SetMaxButton��#��[ $��2��$��0,3,0,3q� . $��58��$��Center=�$��Center=�.���� -   $	 MaxButtonLabel��$��Set Max��$��Center=��� $��0,0,0,10q�$��Stretch=�$��1��$	��Top=�.+��� -   $��SliderControl��  MaterialDesignSlider#��[ {�.   � Value$ 0Q� � SmallChange$ 1Q� � Minimum$ 0Q� � Maximum$ 1Q� �� Orientation$ 
Horizontal=�$��Center=�� $��0,0,0,5q�$��Stretch=�$��Bottom=�$��Stretch=���Z�  �� $��* ��� $��* �.+��� -   $ TestMinMaxButton��$��0��#��[  . $��Right=�$��Center=�$	��6,0q�$��82��.����  �� Orientation$ 
Horizontal=�.+��� $��Test Min/Max��$��24��$	��0,4q�$��78��$��Center=��� -   $ TestValueButton��$��1��#��[  . $
��Left=�$��Center=�$	��6,0q�$��82��.���� $ 
Horizontal=�.+��� $��
Test Value��$��24��$	��0,4q�$��78��$��Center=��� $��6��
������ $ 
Horizontal=�$��Center=�.+��� $��Min: ��$��5,0,0,0q�$��36��$��24�����	#FF999999$
��Bold"�$��Center=��� -   $��lblCurrentMinValue��$�� ��$��40��$��24��$��0,0,10,0q�
��������� $��Max: ��$��10,0,0,0q�$��36��$��24�����	#FF999999$
��Bold"�$��Center=��� -   $��lblCurrentMaxValue��$�� ��$��40��$��24��$��0,0,5,0q�
��������� $��3������  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFFAF9F5��  � Offset$  0.098Q�� $ 	#E5858585��$  1Q�.@��� -   ! ��Name$! 
SBPortName��$��ServoController: ��$
��Left=�$��0,-2,0,0q��� .���� -   " ��Name$" TextServoController��$�� ��$
��Wrap=�$	��250��$��-2,0,0,0q��� $
��Left=�$	��5,2q��� .���� -   $" TextTime��$��Time (ms): ��$
��Wrap=�$��58��$��0,-2,0,0q��� .���� -   $ 	SBTimeBox��$
��2000��$��45��$��0,-2,0,0q��� $
��Left=�$	��5,2q��� .���� -   $" TextInfo��$�� ��$
��Wrap=�$��0,-2,0,0q�