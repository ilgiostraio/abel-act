   M S B A M L   `   `   `  ����   Act.Face.FACEGui20'    !Act.Face.FACEGui20.SettingsDialog   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Settings��$	��430��$	��550�� � 
ResizeMode$ NoResize=� � Icon$) #/Images/ToolbarMenu/Settings-32.png������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF000000�� � Offset$ 0Q�� $ 	#FFFFFFFF��$ 1Q�.��� -    �Name$ 
GridLayout�� � RowDefinitions �� $	��470 ��� $��* �.+��� $��0��   TabControlStyle#��[  $��
20,20,20,0q�.@���   TabItemStyle#��[ $��ComPort��$y�Hidden=�{�.  .���� $��0��$��
10,10,10,0q�$��Normal$�$y�Hidden=����� $��OPTIONS��$��11(�$��Italic$�$
��Bold"�.��� $��5q� �� $��45 ��� $��45 ��� $��45 ��� $��45 ��� $��45 ��� $��45 ��� $��45 ��� $��45 �	 � ColumnDefinitions	 �� $	��110 ��� $��* �.+��� $��0��$��Standard Type��$
��Auto��$��Right=�$��0,0,0,0q�$��Center=��� -   
 ��Name$
 standardCombo��$��0��$��1��$
��20,0q� �� MaxDropDownHeight$	 267��$��Center=��� $��1��$��COM Port��$
��Auto��$��Right=�$��0,0,0,0q�$��Center=��� -   $��comPortCombo��$��1��$��1��$
��20,0q�$	 130��$��Center=��� $��2��$��Bits per second��$
��Auto��$��Right=�$��Center=��� -   $��bitRateCombo��$��2��$��1��$
��20,0q�$	 130��$��Center=��� $��3��$��	Data Bits��$
��Auto��$��Right=�$��0,0,0,0q�$��Center=��� -   $��dataBitsCombo��$��3��$��1��$
��20,0q�$	 130��$��Center=��� $��4��$��Parity��$
��Auto��$��Right=�$��Center=��� -   $��parityCombo��$��4��$��1��$
��20,0q�$	 130��$��Center=��� $��5��$��	Stop Bits��$
��Auto��$��Right=�$��0,0,0,0q�$��Center=��� -   $��stopBitsCombo��$��5��$��1��$
��20,0q�$	 130��$��Center=��� $��6��$��Handshaking��$
��Auto��$��Right=�$��Center=��� -	   $��handshakingCombo��$��6��$��1��$
��20,0q�$	 130��$��Center=��� -
   $��RestoreButton��$��7��$��1��  ButtonStyle#��[ $��26��$��75��$��Restore��$��Center=�$��Right=�$
��20,0q��� #��[ $��Webcam�� �� 
IsSelected . .��� $	��350�� �� $	��370 ��� $��* �.+��� $��0��$��
10,10,10,5q�$��Normal$����� $��WEBCAM OPTIONS��$��11(�$��Italic$�$
��Bold"�.��� $��5,5,5,10q� �� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 ��� $��50 �	 �� $��65 ��� $��* �.+��� $��0��$��Webcam��$
��Auto��$��Right=�$��Center=��� -   $
 webcamCombo��$��0��$��1��$��	10,0,10,0q�$	 267��$��Center=��� -   $��1��#��[ $��26��$��Restore��$��Center=�$��Right=�$��0,0,20,0q�$��75��]� $��1��$��1q���Z� .+��� -   $��ApplyButton��#��[ $��26��$��75��$��Apply��$��Center=�$��Right=�$��0,0,20,0q�$��Right=��� -   $��CancelButton��#��[ $��26��$��75��$��Cancel��$��Center=�$��Right=�$��0,0,5,0q�$��Right=��� -   $��OkButton��#��[ $��26��$��75��$��Ok��$��Center=�$��Right=�$��Right=�$��0,0,5,0q�