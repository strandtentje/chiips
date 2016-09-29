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

Partial Public Class connectionUI
    Public Sub New(ByVal conPin As Chip.ChIIpsSDK.Connection, ByVal IsOutput As Boolean, ByVal newParentChip As ElementContainer)
        MyBase.New()

        Me.InitializeComponent()

        ' Insert code required on object creation below this point.

        Connection = conPin

        AddHandler Connection.Change, AddressOf Switched

        If IsOutput Then MoveBubbleRight()
        If Not IsOutput Then AllowManualSwitch = True

        lblName.Content = Connection.Name

        Switched(conPin)

        ParentChip = newParentChip
        ParentChipB = ParentChip.MyElement
    End Sub

    Sub MoveBubbleRight()
        ellipse.HorizontalAlignment = Windows.HorizontalAlignment.Right

        lblName.HorizontalAlignment = Windows.HorizontalAlignment.Right
        lblName.Margin = New Thickness(0, 0, 20, 0)
    End Sub

    Public ParentChip As ElementContainer
    Public ParentChipB As Chip.ChIIpsSDK.Chip

    Public Event ConnectionClick(ByVal Sender As connectionUI)

    Public Connection As Chip.ChIIpsSDK.Connection
    Public AllowManualSwitch As Boolean

    Public Function GetIndex() As Integer

        If ParentChipB.Inputs.Contains(Connection) Then
            Return ParentChipB.Inputs.IndexOf(Connection)
        End If

        If ParentChipB.Outputs.Contains(Connection) Then
            Return ParentChipB.Outputs.IndexOf(Connection)
        End If

        Return -1

    End Function

    Private Sub Switched(ByVal Pin As Chip.ChIIpsSDK.Connection)
        If Pin.State = True Then
            ellipse.Fill = New SolidColorBrush(Colors.Green)
        Else
            ellipse.Fill = New SolidColorBrush(Colors.Red)
        End If
    End Sub

    Private Sub ellipse_MouseRightButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ellipse.MouseLeftButtonUp
        If AllowManualSwitch Then
            Connection.State = Not Connection.State
            If Connection.State = True Then
                ellipse.Fill = New SolidColorBrush(Colors.Green)
            Else
                ellipse.Fill = New SolidColorBrush(Colors.Red)
            End If
        End If
    End Sub

    Private Sub ellipse_MouseRightButtonUp1(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles ellipse.MouseRightButtonUp
        RaiseEvent ConnectionClick(Me)
    End Sub

End Class
