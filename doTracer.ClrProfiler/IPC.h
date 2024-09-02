#pragma once

#include <atldef.h>
#include <Windows.h>


class NamedPipe
{
public:
    NamedPipe() noexcept;
    ~NamedPipe() noexcept;

    NamedPipe(_In_ const NamedPipe&) = delete; // Copy constructor
    NamedPipe(_In_ NamedPipe&& pipe) noexcept; // Move constructor

    NamedPipe& operator=(_In_ const NamedPipe&) = delete; // Copy assignment operator
    NamedPipe& operator=(_In_ NamedPipe&& prov) noexcept; // Move assignment operator

    operator HANDLE() const noexcept; //  Conversion operator.

    VOID Close() noexcept;
    VOID Attach(_In_opt_ HANDLE hPipe) noexcept;
    _NODISCARD HANDLE Detach() noexcept;
    _NODISCARD BOOL IsOpen() const noexcept;
    BOOL Connect(_Inout_opt_ LPOVERLAPPED lpOverlapped = nullptr) noexcept;
    BOOL Disconnect() noexcept;
    BOOL Flush() noexcept;
    BOOL ImpersonateClient() noexcept;

    BOOL Create(_In_ LPCTSTR lpName, _In_ DWORD dwOpenMode, _In_ DWORD dwPipeMode,
        _In_ DWORD dwMaxInstances, _In_ DWORD dwOutBufferSize,
        _In_ DWORD dwInBufferSize, _In_ DWORD dwDefaultTimeOut,
        _In_opt_ LPSECURITY_ATTRIBUTES lpSecurityAttributes = nullptr) noexcept;

    BOOL Open(_In_ LPCTSTR lpName, _In_ DWORD dwDesiredAccess, _In_ DWORD dwShareMode,
        _In_opt_ LPSECURITY_ATTRIBUTES lpSecurityAttributes = nullptr,
        DWORD dwFlagsAndAttributes = 0) noexcept;

    BOOL Write(_In_reads_bytes_opt_(nNumberOfBytesToWrite) LPCVOID lpBuffer,
        _In_ DWORD nNumberOfBytesToWrite, _Out_opt_ LPDWORD lpNumberOfBytesWritten = nullptr,
        _Inout_opt_ LPOVERLAPPED lpOverlapped = nullptr) noexcept;

    BOOL WriteEx(_In_reads_bytes_opt_(nNumberOfBytesToWrite) LPCVOID lpBuffer,
        _In_ DWORD nNumberOfBytesToWrite, _Inout_ LPOVERLAPPED lpOverlapped,
        _In_ LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine) noexcept;

    BOOL Read(_Out_writes_bytes_to_opt_(nNumberOfBytesToRead, *lpNumberOfBytesRead)
        __out_data_source(FILE) LPVOID lpBuffer, _In_ DWORD nNumberOfBytesToRead,
        _Out_opt_ LPDWORD lpNumberOfBytesRead = nullptr,
        _Inout_opt_ LPOVERLAPPED lpOverlapped = nullptr) noexcept;

    BOOL ReadEx(_Out_writes_bytes_opt_(nNumberOfBytesToRead) __out_data_source(FILE) LPVOID lpBuffer,
        _In_ DWORD nNumberOfBytesToRead, _Inout_ LPOVERLAPPED lpOverlapped,
        _In_ LPOVERLAPPED_COMPLETION_ROUTINE lpCompletionRoutine) noexcept;

    BOOL Peek(_Out_writes_bytes_to_opt_(nBufferSize, *lpBytesRead) LPVOID lpBuffer,
        _In_ DWORD nBufferSize, _Out_opt_ LPDWORD lpBytesRead = nullptr,
        _Out_opt_ LPDWORD lpTotalBytesAvail = nullptr,
        _Out_opt_ LPDWORD lpBytesLeftThisMessage = nullptr) noexcept;

    BOOL Transact(_In_reads_bytes_opt_(nInBufferSize) LPVOID lpInBuffer, _In_ DWORD nInBufferSize,
        _Out_writes_bytes_to_opt_(nOutBufferSize, *lpBytesRead) LPVOID lpOutBuffer,
        _In_ DWORD nOutBufferSize, _Out_ LPDWORD lpBytesRead,
        _Inout_opt_ LPOVERLAPPED lpOverlapped = nullptr) noexcept;

    BOOL GetState(_Out_opt_ LPDWORD lpState, _Out_opt_ LPDWORD lpCurInstances,
        _Out_opt_ LPDWORD lpMaxCollectionCount, _Out_opt_ LPDWORD lpCollectDataTimeout,
        _Out_writes_opt_(nMaxUserNameSize) LPTSTR lpUserName, _In_ DWORD nMaxUserNameSize) noexcept;

    BOOL GetInfo(_Out_opt_ LPDWORD lpFlags, _Out_opt_ LPDWORD lpOutBufferSize = nullptr,
        _Out_opt_ LPDWORD lpInBufferSize = nullptr, _Out_opt_ LPDWORD lpMaxInstances = nullptr) noexcept;

    BOOL SetState(_In_opt_ LPDWORD lpMode, _In_opt_ LPDWORD lpMaxCollectionCount,
        _In_opt_ LPDWORD lpCollectDataTimeout) noexcept;

    __if_exists(SetNamedPipeHandleState)
    {
        _Success_(return != 0)
            BOOL GetClientComputerName(_Out_writes_bytes_(ClientComputerNameLength) LPTSTR ClientComputerName,
                _In_ ULONG ClientComputerNameLength) noexcept
        {
            ATLASSERT(IsOpen());

            return GetNamedPipeClientComputerName(m_hPipe, ClientComputerName, ClientComputerNameLength);
        }
    }

    __if_exists(GetNamedPipeClientProcessId)
    {
        _Success_(return != 0)
            BOOL GetClientProcessId(_Out_ PULONG ClientProcessId) noexcept
        {
            ATLASSERT(IsOpen());

            return GetNamedPipeClientProcessId(m_hPipe, ClientProcessId);
        }
    }

    __if_exists(GetNamedPipeClientSessionId)
    {
        _Success_(return != 0)
            BOOL GetClientSessionId(_Out_ PULONG ClientSessionId) noexcept
        {
            ATLASSERT(IsOpen());

            return GetNamedPipeClientSessionId(m_hPipe, ClientSessionId);
        }
    }

    __if_exists(GetNamedPipeServerProcessId)
    {
        _Success_(return != 0)
            BOOL GetServerProcessId(_Out_ PULONG ServerProcessId) noexcept
        {
            ATLASSERT(IsOpen());

            return GetNamedPipeServerProcessId(m_hPipe, ServerProcessId);
        }
    }

    __if_exists(GetNamedPipeServerSessionId)
    {
        _Success_(return != 0)
            BOOL GetServerSessionId(_Out_ PULONG ServerSessionId) noexcept
        {
            ATLASSERT(IsOpen());

            return GetNamedPipeServerSessionId(m_hPipe, ServerSessionId);
        }
    }

protected:
    HANDLE m_hPipe;
};
