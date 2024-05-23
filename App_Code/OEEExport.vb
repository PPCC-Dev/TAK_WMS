Imports Microsoft.VisualBasic

Public Class OEEExport

    Private Shared _UserName As String
    Private Shared _Employee As String
    Private Shared _LineStarting As String
    Private Shared _LineEnding As String
    Private Shared _ResourceStarting As String
    Private Shared _ResourceEnding As String
    Private Shared _WcStarting As String
    Private Shared _WcEnding As String
    Private Shared _JobStarting As String
    Private Shared _JobEnding As String
    Private Shared _ItemStarting As String
    Private Shared _ItemEnding As String
    Private Shared _DateStarting As String
    Private Shared _DateEnding As String
    Private Shared _ShiftStarting As String
    Private Shared _ShiftEnding As String

    Public Shared Property UserName As String
        Get
            Return _UserName
        End Get
        Set(value As String)
            _UserName = value
        End Set
    End Property

    Public Shared Property Employee As String
        Get
            Return _Employee
        End Get
        Set(value As String)
            _Employee = value
        End Set
    End Property

    Public Shared Property LineStarting As String
        Get
            Return _LineStarting
        End Get
        Set(value As String)
            _LineStarting = value
        End Set
    End Property

    Public Shared Property LineEnding As String
        Get
            Return _LineEnding
        End Get
        Set(value As String)
            _LineEnding = value
        End Set
    End Property

    Public Shared Property ResourceStarting As String
        Get
            Return _ResourceStarting
        End Get
        Set(value As String)
            _ResourceStarting = value
        End Set
    End Property

    Public Shared Property ResourceEnding As String
        Get
            Return _ResourceEnding
        End Get
        Set(value As String)
            _ResourceEnding = value
        End Set
    End Property

    Public Shared Property WcStarting As String
        Get
            Return _WcStarting
        End Get
        Set(value As String)
            _WcStarting = value
        End Set
    End Property

    Public Shared Property WcEnding As String
        Get
            Return _WcEnding
        End Get
        Set(value As String)
            _WcEnding = value
        End Set
    End Property

    Public Shared Property JobStarting As String
        Get
            Return _JobStarting
        End Get
        Set(value As String)
            _JobStarting = value
        End Set
    End Property

    Public Shared Property JobEnding As String
        Get
            Return _JobEnding
        End Get
        Set(value As String)
            _JobEnding = value
        End Set
    End Property

    Public Shared Property ItemStarting As String
        Get
            Return _ItemStarting
        End Get
        Set(value As String)
            _ItemStarting = value
        End Set
    End Property

    Public Shared Property ItemEnding As String
        Get
            Return _ItemEnding
        End Get
        Set(value As String)
            _ItemEnding = value
        End Set
    End Property

    Public Shared Property DateStarting As String
        Get
            Return _DateStarting
        End Get
        Set(value As String)
            _DateStarting = value
        End Set
    End Property

    Public Shared Property DateEnding As String
        Get
            Return _DateEnding
        End Get
        Set(value As String)
            _DateEnding = value
        End Set
    End Property

    Public Shared Property ShiftStarting As String
        Get
            Return _ShiftStarting
        End Get
        Set(value As String)
            _ShiftStarting = value
        End Set
    End Property

    Public Shared Property ShiftEnding As String
        Get
            Return _ShiftEnding
        End Get
        Set(value As String)
            _ShiftEnding = value
        End Set
    End Property
End Class
