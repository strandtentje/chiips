Imports System
Imports System.Collections.Generic
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Threading

Partial Public Class ElementContainer
    Public Sub New(ByVal element As Chip.ChIIpsSDK.Chip, ByVal PreviewMode As Boolean, ByVal MainWin As MainWindow, ByVal ParentGrid As Grid, ByVal GivenID As String)
        MyBase.New()

        Me.InitializeComponent()

        ' Insert code required on object creation below this point.

        MyElement = element

        _MainWin = MainWin

        _Preview = PreviewMode

        Dim tmpConnection As connectionUI

        If Not ParentGrid Is Nothing Then
            ID = element.Name & ParentGrid.Children.Count
        End If

        If Not GivenID = "" Then
            ID = GivenID
        End If

        For Each Pin As Chip.ChIIpsSDK.Connection In element.Inputs
            tmpConnection = New connectionUI(Pin, False, Me)
            If Not PreviewMode Then AddHandler tmpConnection.ConnectionClick, AddressOf MainWin.TargetConnectionClick
            grdInputs.Children.Add(tmpConnection)
        Next

        For Each Pin As Chip.ChIIpsSDK.Connection In element.Outputs
            tmpConnection = New connectionUI(Pin, True, Me)
            If Not PreviewMode Then AddHandler tmpConnection.ConnectionClick, AddressOf MainWin.SourceConnectionClick
            grdOutputs.Children.Add(tmpConnection)
        Next

        Me.Width = DirectCast(element.UI, Control).Width + 52
        Me.Height = DirectCast(element.UI, Control).Height + 8

        Dim iHeight As Integer = grdInputs.Children.Count * 20 + 8
        Dim oHeight As Integer = grdOutputs.Children.Count * 20 + 8

        If iHeight > Me.Height Then
            Me.Height = iHeight
        End If

        If oHeight > Me.Height Then
            Me.Height = oHeight
        End If

        grdElement.Child = element.UI

        If Not HorizontalAlignment = Windows.HorizontalAlignment.Left Then

            HorizontalAlignment = Windows.HorizontalAlignment.Left
            VerticalAlignment = Windows.VerticalAlignment.Top

        End If
    End Sub

    Public Event Choose(ByVal Element As Chip.ChIIpsSDK.Chip, ByVal Container As ElementContainer)
    Public Event DupeChoose(ByVal Element As Chip.ChIIpsSDK.Chip, ByVal Container As ElementContainer)

    Dim _MainWin As MainWindow

    Public ID As String
    Public MyElement As Chip.ChIIpsSDK.Chip
    Dim _Preview As Boolean

    Dim mdn As Boolean
    Dim px As Integer
    Dim py As Integer

    Public Function GetSourcePinByID(ByVal ID As String) As connectionUI

        For Each Connection As connectionUI In grdOutputs.Children
            If Connection.Connection.Name.ToLower = ID Then
                Return Connection
            End If
        Next

        Return Nothing

    End Function

    Public Function GetTargetPinByID(ByVal ID As String) As connectionUI

        For Each Connection As connectionUI In grdInputs.Children
            If Connection.Connection.Name.ToLower = ID Then
                Return Connection
            End If
        Next

        Return Nothing

    End Function

    Private Sub ElementContainer_MouseDoubleClick(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles cmdRemove.Click
        If Not Keyboard.Modifiers = ModifierKeys.Control Then
            RaiseEvent Choose(MyElement, Me)
        Else
            RaiseEvent DupeChoose(MyElement, Me)
        End If
    End Sub

    Private Sub ElementContainer_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Me.MouseDown
        If (Not _Preview) Then
            px = e.GetPosition(Me).X
            py = e.GetPosition(Me).Y
            w = Me.Width
            h = Me.Height
            mdn = True
            timMove.Start()
        End If
    End Sub

    Dim w As Integer
    Dim h As Integer

    Dim WithEvents timMove As New DispatcherTimer With {.Interval = New TimeSpan(0, 0, 0, 0, 20)}

    Sub Timer() Handles timMove.Tick
        If mdn And Mouse.LeftButton = MouseButtonState.Pressed Then
            Me.Margin = New Thickness(Me.Margin.Left + Mouse.GetPosition(Me).X - px, Me.Margin.Top + Mouse.GetPosition(Me).Y - py, 0, 0)
            Me.Width = w
            Me.Height = h
            _MainWin.Movement()
        Else
            mdn = False
            timMove.Stop()
        End If
    End Sub


End Class
