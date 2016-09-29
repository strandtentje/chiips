Imports System.Windows

Namespace ChIIpsSDK

    Public Class Chip

        Public Sub New()

            AddHandler Outputs.ChangeComitted, AddressOf ChangeInOutputs
            AddHandler Inputs.ChangeComitted, AddressOf ProcessChanges

        End Sub

        Public Overridable ReadOnly Property Bool As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable ReadOnly Property FP As Boolean
            Get
                Return False
            End Get
        End Property

        Public Inputs As New ConnectionCollection
        Public Outputs As New ConnectionCollection

        Public Event OutputChange(ByVal Chip As Chip, ByVal Output As Connection)

        Public Sub ChangeInOutputs(ByVal Output As Connection)

            RaiseEvent OutputChange(Me, Output)

        End Sub

        Public Overridable Sub ProcessChanges()

        End Sub

        Public Overridable ReadOnly Property UI() As UIElement
            Get
                Return New EmptyUI(Name)
            End Get
        End Property

        Public Overridable ReadOnly Property Name() As String
            Get
                Return "Naamloos Element"
            End Get
        End Property

        Public Overridable ReadOnly Property Classification() As String
            Get
                Return "General"
            End Get
        End Property

    End Class

    Public Class Connection

        Public PerhapsTheUI As Windows.Controls.Control


        Dim _Name As String
        Dim _State As Double

        Public Sub New(ByVal newName As String)

            _Name = newName

        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return _Name
            End Get
        End Property

        Public Property State() As Double
            Get
                Return _State
            End Get
            Set(ByVal value As Double)
                If value <> _State Then
                    _State = value
                    RaiseEvent Change(Me)
                End If
            End Set
        End Property

        Public Event Change(ByVal whatInput As Connection)

    End Class

    Public Class ConnectionCollection

        Inherits ObjectModel.Collection(Of Connection)

        Public Overloads Sub Add(ByVal Name As String)

            Dim newItem As New Connection(Name)

            AddHandler newItem.Change, AddressOf Change

            MyBase.Add(newItem)

        End Sub

        Private Sub Change(ByVal Input As Connection)
            RaiseEvent ChangeComitted(Input)
        End Sub

        Public Event ChangeComitted(ByVal Input As Connection)

        Public Function GetByName(ByVal Name As String) As Connection
            For Each Connection As Connection In Me
                If Connection.Name.ToLower = Name Then
                    Return Connection
                End If
            Next
            Return Nothing
        End Function

    End Class

End Namespace