   M S B A M L   `   `   `  ���� U  QAct.Lib.ControllersLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibraryAct.Lib.ControllersLibrary  M IAct.Face.FACEGui20, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:Act.Face.FACEGui20.ViewAct.Face.FACEGui20.View =#clr-namespace:Act.Face.FACEGui20.UIAct.Face.FACEGui20.UI %   Act.Face.FACEGui20.FACEGui20Win   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ^localLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibrary     6view%clr-namespace:Act.Face.FACEGui20.View     2ui#clr-namespace:Act.Face.FACEGui20.UI     -   ���	#FF000000  � Title$  FACEGui��$
��1280��$
��1280��$	��778��$	��750�� � WindowStartupLocation$ CenterScreen=� � AllowsTransparency .  $y�Visible=� � OverridesDefaultStyle .   � 
ResizeMode$ NoResize=���Z� .��� ����  �� ImageSource$ Images/sfondo.jpg�� � RowDefinitions �� $��28 ��� $��34 ��� $��* �$��1��$��4���� $��* ��� $��24 �.+�#  Act.Face.FACEGui20.UI.TopMenu  #  Act.Face.FACEGui20.UI.TopMenu!   RobotSelectionChangedEvent$ RobotSelectionChanged��$��0��#  Act.Face.FACEGui20.UI.ToolBar  $��1���� $��2��$��1��$
��Auto��$��Stretch=�$��Stretch=�{�. ���	#FF000000����  �� EndPoint$ 0.5,1&�	 �� 
StartPoint$	 0.5,0&�.��� 
 � Color$
 	#FFFFFFFF�� � Offset$ 0Q�� $
 	#FFFFFFFF��$ 1Q�� $��3��$
��Auto��$
��Auto��$��0,0,0,0q�$��Stretch=� � ColumnDefinitions �� $��63 ��� $��2 ��� $��* �.+�� -   $��LeftContentGrid��$��0��! �� IsDeferredScrollingEnabled .  $��0,2,0,0q�.+�$  Act.Face.FACEGui20.UI.LeftMenu  $  Act.Face.FACEGui20.UI.LeftMenu   LookAtMouseClickOnCanvas$ LookAtEventHandler��   ECSMouseClickOnCanvas$ ECSEventHandler���� $��1��$��0��$
��Auto��$��1��$��Stretch=�$��Stretch=�{�. ���	#FF000000���� $ 0.5,1&�$	 0.5,0&�.��� $
 	#FFFFFFFF��$ 0Q�� $
 	#FFFFFFFF��$ 1Q�� -   $��CentralContentGrid��$��2��$	��0,1q�.+�� -   $��ViewGrid��$y�Hidden=�
�������.+�&   Act.Face.FACEGui20.View.ViewView  � -   $��EditGrid��.+�)  #Act.Face.FACEGui20.View.FACEditView  -     Name$ FACEView��$y�Visible=�� -   $��EditGridAbel��.+�*  $Act.Face.FACEGui20.View.AbelEditView  -     Name$ AbelView��$y�Hidden=�� -	   $��NetGrid��$y�Hidden=�.+�%	  Act.Face.FACEGui20.View.NetView	  -
    	 Name$ yarpView��� -   $��TestGrid��$y�Hidden=�.+�� $��0��$��0,0,0,1q�$��3�� �� $	��350 ��� $��2 ��� $��* �.+��� -   $��PersonalDataGroupbox��$��0��$��5q����� $��DATI PERSONALI��$��13(�$��Italic$�$
��Bold"�.����    LeftScrollViewer#��[  ���	#FF6D6D6D.���� -   $��	LeftPanel��.+��� -   $��PersonalDataPanel��.+���  �� Orientation$ 
Horizontal=�$��5q�.+��� $
��Nome��$
��Bold"��� -   $��TextboxName��$	��278��$�� ��$��Right=�$��Center=��� $ 
Horizontal=�$��5q�.+��� $��Cognome��$
��Bold"��� -   $��TextboxSurname��$	��258��$�� ��$��Center=��� $ 
Horizontal=�$��5q�.+��� $��Data di nascita��$
��Bold"��� -   $��
TextboxDay��$��30��$�� ��$��Center=�$��Center=��� $��/��$
��Bold"��� -   $��TextboxMonth��$��30��$�� ��$��Center=�$��Center=��� $��/��$
��Bold"��� -   $��TextboxYear��$��40��$�� ��$��Center=�$��Center=��� $ 
Horizontal=�$��5q�.+��� $��Sesso��$
��Bold"�	� -   $��MRadioButton��$��5,5,0,5q�$��Center=��� $��M��	� -   $��FRadioButton��$��5,5,0,5q�$��Center=��� $��F���� $ 
Horizontal=�$��5q�.+��� $��Facoltà��$
��Bold"��� -   $��TextboxFaculty��$	��194��$�� ��$��Center=��� $ 
Horizontal=�$��5q�.+��� $��Professione��$
��Bold"��� -   $��
TextboxJob��$	��194��$�� ��$��Center=��� $ 
Horizontal=�$��5q�.+���  d� 	IsChecked$ FalseY�$��Center=��� $G��ADichiaro di aver letto e firmato il foglio di consenso informato.��$��10(� �� TextWrapping$
 Wrap=��� -   $��RestTestButton��  ButtonStyle#��[ $��10q�$��75��$��Center=�$��Center=�.���� $��Fase riposo���� -   $��ClockTextbox��$	��150��$��30��$��10q�$��0,-4,0,0q�$��00:00:00��$��22(� �� TextAlignment$ Center=��� -   $��GotoTestButton��#��[ {�.  $��10q�$��75��$��Center=�$��Center=�.���� $��Prosegui���� $��1��$
��Auto��$��1��$��Stretch=�$��Stretch=�{�. ���	#FF000000���	#FFAAAAAA$	��0,3q��� $��2��$��5q����� $
��TEST��$��13(�$��Italic$�$
��Bold"�.���� #��[  ���	#FF6D6D6D.���� -   $��	TestPanel��{�.  .+��� -   $��StartTestButton��#��[ $��10q�$��75��$��Center=�$��Center=�.���� $��Start��� $��10q� � ShowGridLines .  �� $��96 ��� $��96 ��� $��96 ��� $��96 ��� $��96 ��� $	��100 ��� $��96 ��� $	��100 � �� $��40 ��� $��40 �.+��� $��0��$��0��$ 
Horizontal=�.+�	� -   0��$��5,5,0,5q�$��Center=��� $��Orgoglio��$��Center=��� $��0��$��1��$ 
Horizontal=�.+�	� -   1��$��5,5,0,5q�$��Center=��� $��	Felicità��$��Center=��� $��0��$��2��$ 
Horizontal=�.+�	� -   2��$��5,5,0,5q�$��Center=��� $��	Imbarazzo��$��Center=��� $��0��$��3��$ 
Horizontal=�.+�	� -    3��$��5,5,0,5q�$��Center=��� $��Neutra��$��Center=��� $��0��$��4��$ 
Horizontal=�.+�	� -!   4��$��5,5,0,5q�$��Center=��� $��Sorpresa��$��Center=��� $��0��$��5��$ 
Horizontal=�.+�	� -"   5��$��5,5,0,5q�$��Center=��� $��Disgusto��$��Center=��� $��0��$��6��$ 
Horizontal=�.+�	� -#   6��$��5,5,0,5q�$��Center=��� $��Dolore��$��Center=��� $��0��$��7��$ 
Horizontal=�.+�	� -$   7��$��5,5,0,5q�$��Center=��� $��Compassione��$��Center=��� $��1��$��0��$ 
Horizontal=�.+�	� -%   8��$��5,5,0,5q�$��Center=��� $��	Disprezzo��$��Center=��� $��1��$��1��$ 
Horizontal=�.+�	� -&   9��$��5,5,0,5q�$��Center=��� $��	Tristezza��$��Center=��� $��1��$��2��$ 
Horizontal=�.+�	� -'   10��$��5,5,0,5q�$��Center=��� $��	Interesse��$��Center=��� $��1��$��3��$ 
Horizontal=�.+�	� -(   11��$��5,5,0,5q�$��Center=��� $��Vergogna��$��Center=��� $��1��$��4��$ 
Horizontal=�.+�	� -)   12��$��5,5,0,5q�$��Center=��� $��Paura��$��Center=��� $��1��$��5��$ 
Horizontal=�.+�	� -*   13��$��5,5,0,5q�$��Center=��� $��Eccitamento��$��Center=��� $��1��$��6��$ 
Horizontal=�.+�	� -+   14��$��5,5,0,5q�$��Center=��� $��Rabbia��$��Center=��� $��1��$��7��$ 
Horizontal=�.+�	� -,   15��$��5,5,0,5q�$��Center=��� $��	Non la so��$��Center=��� --   $��NextTestButton��#��[ $��10q�$��75��$��Center=�$��Right=�.���� $
��Next���� -.   $��StopTestButton��#��[ $��10q�$��75��$��Center=�$��Center=�.���� $
��Stop���� $��4�����	#FF989896$��24��.@��� $��0,0,0,0q�.���� -/    ��Name$ TextTime��$��TimeExp. (ms) ��$
��Wrap=��� $��0,0,0,0q�.���� -0    ��Name$ 	SBTimeBox��$	��500��$��40��$��-2q��� $��0,0,0,0q�.���� -1   $ TextTimeNeck��$��TimeNeck (ms) ��$
��Wrap=��� $��0,0,0,0q�.���� -2   $ SBTimeNeckBox��$
��1000��$��40��$��-2q��� $��5,0,5,0q�.���� -3    ��Name$ CheckboxAnimator��$��Animator��$��Center=�$��Center=�$��Normal"�$ FalseY��� $��5,0,5,0q�.���� -4   $ CheckboxYarpExp��$��Yarp Expression��$��Center=�$��Center=�$��Normal"�$ FalseY��� $
��Left=�$	��5,2q��� $��0,0,0,0q�.���� -5   $ 	TextSpeed��$��Speed (pos/s) ��$
��Wrap=��� $��0,0,0,0q�.���� -6   $ txtSpeed��$	��0.8��$��40��$��-2q��� $��5,0,5,0q�.���� -7   $ ckSpeed��$��Speed��$��Center=�$��Center=�$��Normal"�$ FalseY��� $
��Left=�$	��5,2q��� $��Right=�$��0,0,0,0q�.���� $ 
Horizontal=�$��0q�$	��Top=�.+�A� -8    A�Name$ 
BarEllyarp��$��10��
���  ��$��10��$��0,0,5,0q��� $��Yarp Server��$��12(�
���   �$��Normal"�$��0q�$��0,0,5,0q�$	��Top=�A� -9   $ BarEllAttention��$��10��
���  ��$��10��$��0,0,5,0q��� $��FACE Robot Control��$��12(�
���   �$��Normal"�$��0q�$��0q� � UseLayoutRounding .  ��Z� �� $
��Left=�$	��5,2q��� $��Right=�$��0,0,0,0q�.���� -:   $ TextTIME��$�� ��$
��Wrap=�$��56���� $
��Left=�$	��5,2q��� $��0,0,0,0q�.���� -;   $ 	TextError��$�� ��$
��Wrap=�$��50���� $
��Left=�$��5,2,5,2q��� .���� -<   $ 	SBInfoBox��$�� ��$
��Wrap=�$	��209��