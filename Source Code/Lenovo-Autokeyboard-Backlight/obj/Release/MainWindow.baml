   M S B A M L   `   `   `  ���� X  TLenovo-Autokeyboard-Backlight, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:Lenovo_Autokeyboard_BacklightLenovo_Autokeyboard_Backlight  .    (Lenovo_Autokeyboard_Backlight.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     =local+clr-namespace:Lenovo_Autokeyboard_Backlight     -     � Title$$  Lenovo Auto Keyboard Backlight��$	��110��$	��320�� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � Icon$ logo.ico�� � 	Resources    StandartMargin
 u� Top u� Bottom u� Left u� Right  StandartFont	 �� 
TargetType  StandartLabelStyle
 �� Property �� Value  StandartExitButtonStyle �� 
TargetType ��Name �� Triggers P� Property P� Value  StandartMinimizeButtonStyle��%�  &	        &	 %     &	 =     7  7   &	 �     7  7   &	 1    7  7   u� $ 5Q�$ 5Q�$ 5Q�$ 5Q�*�Lucida Console)��� "	 ��.��� 
 � ��$ Center=��� 
 � ��$ Center=��� 
 � ��$ Stretch=��� 
 � ��$ Stretch=��� 
 � ��
 ������� 
 � ��8   �� 
 � ��8  �� "	 ��.��� 
 � ��$ Stretch=��� 
 � ��$ Stretch=��� 
 � ��
 ������� 
 � ��8   �� 
 � ��8  �� 
 � �� �� " ��.R��� $ Overlay��
���  ��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� 
 � �� �	#FFFF8484�� 
 �@�Cursor$
 Hand���� "	 ��.��� 
 � ��$ Stretch=��� 
 � ��$ Stretch=��� 
 � ��
 ������� 
 � ��8   �� 
 � ��8  �� 
 � �� �� " ��.R��� $ Overlay�����	#FF16168B._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� 
 � �� �	#FFFF8484�� 
 �@�Cursor$
 Hand��.���� -   $ WindowBorder��$��1q����	#FF2F384B._�� ���	#FF202225 � RowDefinitions �� $��25 ��� $��75 � � ColumnDefinitions �� $��10 ��� $	��300 ��� $��10 �.+�� -    �Name$ DragBarGrid�����	#FF202225$��4�� �� $��* � �� $��10 ��� $��50 ��� $��* ��� $��25 ��� $��25 �.+��� $��1��$��Stretch=�$��Stretch=�$��logo.ico��$��1q��� $��2��$��2��$$��Lenovo Auto Keyboard Backlight��$
��Left=�$��Center=�$��10(�#��[ 
�������$��0q��� -    ��Name$ 
ExitButton��$��4��$��X��$��Center=�$��Center=�$��20��$��20��$��10(�#��[ �� -   $ MinimizeButton��$��3��$��_��$��Center=�$��Center=�$��20��$��20��$��10(�#��[ � $��1��$��1�����	#FF2F3136 �� $��25 ��� $��25 ��� $��25 ��� $��* � �� $��1* ��� $��1* �.+��� -    ��Name$ TopTextBlock��$��0��$��0��$��2q�$��Step down delay (ms)��$��10(�#��[ �� -    ��Name$ DelayTextBox��$��0��$��1��$��2q� �� TextWrapping$
 Wrap=�$
��5000�����	#FF919191�� -   $ TopStatusTextBlock��$��1��$��0��$��2q�$��Light Status��$��10(�#��[ �� -	    ��Name$ StatusSlider��$��1��$��1��$��Center=�$��Center=�$��75�� � Minimum$ 0Q� � Maximum$ 2Q� �� TickPlacement$ BottomRight=�{�.  �� -
   $ StatusTextBlock��$��1��$��2��$��2q�$	��Off��$��10(�#��[ 