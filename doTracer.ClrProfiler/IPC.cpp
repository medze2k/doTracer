#include "IPC.h"

NamedPipe::NamedPipe() noexcept : m_hPipe{ INVALID_HANDLE_VALUE }
{
}

NamedPipe::~NamedPipe() noexcept
{
    Close();
}

NamedPipe::NamedPipe(_In_ NamedPipe&& pipe) noexcept : m_hPipe{ INVALID_HANDLE_VALUE }
{
    Attach(pipe.Detach());
}

NamedPipe& NamedPipe::operator=(_In_ NamedPipe&& prov) noexcept
{
    if (m_hPipe != INVALID_HANDLE_VALUE)
        Close();

    Attach(prov.Detach());
    return *this;
}

NamedPipe::operator HANDLE() const noexcept
{
    return m_hPipe;
}

VOID NamedPipe::Close() noexcept
{
    if (m_hPipe != INVALID_HANDLE_VALUE)
    {
        CloseHandle(m_hPipe);
        m_hPipe = INVALID_HANDLE_VALUE;
    }
}

VOID NamedPipe::Attach(_In_opt_ HANDLE hPipe) noexcept
{
    Close();
    m_hPipe = hPipe;
}

_NODISCARD HANDLE NamedPipe::Detach() noexcept
{
    HANDLE hReturn{ m_hPipe };
    m_hPipe = INVALID_HANDLE_VALUE;
    return hReturn;
}

_NODISCARD BOOL NamedPipe::IsOpen() const noexcept
{
    return (m_hPipe != INVALID_HANDLE_VALUE);
}

BOOL NamedPipe::Connect(_Inout_opt_ LPOVERLAPPED lpOverlapped) noexcept
{
    ATLASSERT(IsOpen());

    return ConnectNamedPipe(m_hPipe, lpOverlapped);
}

BOOL NamedPipe::Disconnect() noexcept
{
    ATLASSERT(IsOpen());

    return DisconnectNamedPipe(m_hPipe);
}

BOOL NamedPipe::Flush() noexcept
{
    ATLASSERT(IsOpen());

    return FlushFileBuffers(m_hPipe);
}

BOOL NamedPipe::ImpersonateClient() noexcept
{
    ATLASSERT(IsOpen());

    return ImpersonateNamedPipeClient(m_hPipe);
}

BOOL NamedPipe::Create(_In_ LPCTSTR lpName, _In_ DWORD dwOpenMode, _In_ DWORD dwPipeMode,
    _In_ DWORD dwMaxInstances, _In_ DWORD dwOutBufferSize, _In_ DWORD dwInBufferSize,
    _In_ DWORD dwDefaultTimeOut, _In_opt_ LPSECURITY_ATTRIBUTES lpSecurityAttributes) noexcept
{
    ATLASSERT(!IsOpen());

    m_hPipe = CreateNamedPipe(lpName, dwOpenMode, dwPipeMode, dwMaxInstances, dwOutBufferSize,
        dwInBufferSize, dwDefaultTimeOut, lpSecurityAttributes);

    return (m_hPipe != INVALID_HANDLE_VALUE);
}

BOOL NamedPipe::Open(_In_ LPCTSTR lpName, _In_ DWORD dwDesiredAccess, _In_ DWORD dwShareMode,
    _In_opt_ LPSECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwFlagsAndAttributes) noexcept
{
    ATLASSERT(!IsOpen());

    m_hPipe = CreateFile(lpName, dwDesiredAccess, dwShareMode, lpSecurityAttributes,
        OPEN_EXISTING, dwFlagsAndAttributes, nullptr);

    return (m_hPipe != INVALID_HANDLE_VALUE);
}

BOOL NamedPipe::Write(_In_reads_bytes_opt_(nNumberOfBytesToWrite) LPCVOID lpBuffer,
    _In_ DWORD nNumberOfBytesToWrite, _Out_opt_ LPDWORD lpNumberOfBytesWritten,
    _Inout_opt_ LPOVERLAPPED lpOverlapped) noexcept
{
    ATLASSERT(IsOpen());

    return WriteFile(m_hPipe, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped);
}

BOOL NamedPipe::WriteEx(_In_reads_bytes_opt_(nNumberOfBytesToWrite) LPCVOID lpBuffer,
    _In_ DWORD nNumberOfBytesToWrite, _Inout_ LPOVERLAPPED lpOverlapped,
    _In_ LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine) noexcept
{
    ATLASSERT(IsOpen());
    return WriteFileEx(m_hPipe, lpBuffer, nNumberOfBytesToWrite, lpOverlapped, lpCompletionRoutine);
}

BOOL NamedPipe::Read(_Out_writes_bytes_to_opt_(nNumberOfBytesToRead, *lpNumberOfBytesRead)
    __out_data_source(FILE) LPVOID lpBuffer, _In_ DWORD nNumberOfBytesToRead,
    _Out_opt_ LPDWORD lpNumberOfBytesRead, _Inout_opt_ LPOVERLAPPED lpOverlapped) noexcept
{
    ATLASSERT(IsOpen());

    return ReadFile(m_hPipe, lpBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped);
}

BOOL NamedPipe::ReadEx(_Out_writes_bytes_opt_(nNumberOfBytesToRead) __out_data_source(FILE) LPVOID lpBuffer,
    _In_ DWORD nNumberOfBytesToRead, _Inout_ LPOVERLAPPED lpOverlapped,
    _In_ LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine) noexcept
{
    ATLASSERT(IsOpen());

    return ReadFileEx(m_hPipe, lpBuffer, nNumberOfBytesToRead, lpOverlapped, lpCompletionRoutine);
}

BOOL NamedPipe::Peek(_Out_writes_bytes_to_opt_(nBufferSize, *lpBytesRead) LPVOID lpBuffer,
    _In_ DWORD nBufferSize, _Out_opt_ LPDWORD lpBytesRead,  _Out_opt_ LPDWORD lpTotalBytesAvail,
    _Out_opt_ LPDWORD lpBytesLeftThisMessage) noexcept
{
    ATLASSERT(IsOpen());

    return PeekNamedPipe(m_hPipe, lpBuffer, nBufferSize, lpBytesRead, lpTotalBytesAvail,
        lpBytesLeftThisMessage);
}

BOOL NamedPipe::Transact(_In_reads_bytes_opt_(nInBufferSize) LPVOID lpInBuffer, _In_ DWORD nInBufferSize,
    _Out_writes_bytes_to_opt_(nOutBufferSize, *lpBytesRead) LPVOID lpOutBuffer, _In_ DWORD nOutBufferSize,
    _Out_ LPDWORD lpBytesRead, _Inout_opt_ LPOVERLAPPED lpOverlapped) noexcept
{
    ATLASSERT(IsOpen());

    return TransactNamedPipe(m_hPipe, lpInBuffer, nInBufferSize, lpOutBuffer, nOutBufferSize,
        lpBytesRead, lpOverlapped);
}

BOOL NamedPipe::GetState(_Out_opt_ LPDWORD lpState, _Out_opt_ LPDWORD lpCurInstances,
    _Out_opt_ LPDWORD lpMaxCollectionCount, _Out_opt_ LPDWORD lpCollectDataTimeout,
    _Out_writes_opt_(nMaxUserNameSize) LPTSTR lpUserName, _In_ DWORD nMaxUserNameSize) noexcept
{
    ATLASSERT(IsOpen());

    return GetNamedPipeHandleState(m_hPipe, lpState, lpCurInstances, lpMaxCollectionCount,
        lpCollectDataTimeout, lpUserName, nMaxUserNameSize);
}

BOOL NamedPipe::GetInfo(_Out_opt_ LPDWORD lpFlags, _Out_opt_ LPDWORD lpOutBufferSize,
    _Out_opt_ LPDWORD lpInBufferSize, _Out_opt_ LPDWORD lpMaxInstances) noexcept
{
    ATLASSERT(IsOpen());

    return GetNamedPipeInfo(m_hPipe, lpFlags, lpOutBufferSize, lpInBufferSize, lpMaxInstances);
}

BOOL NamedPipe::SetState(_In_opt_ LPDWORD lpMode, _In_opt_ LPDWORD lpMaxCollectionCount,
    _In_opt_ LPDWORD lpCollectDataTimeout) noexcept
{
    ATLASSERT(IsOpen());

    return SetNamedPipeHandleState(m_hPipe, lpMode, lpMaxCollectionCount, lpCollectDataTimeout);
}
