#include "ValueReader.h"

extern ICorProfilerInfo8* g_corProfilerInfo;

// Check memory address access
const DWORD dwForbiddenArea = PAGE_GUARD | PAGE_NOACCESS;
const DWORD dwReadRights = PAGE_READONLY | PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READ | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY;
const DWORD dwWriteRights = PAGE_READWRITE | PAGE_WRITECOPY | PAGE_EXECUTE_READWRITE | PAGE_EXECUTE_WRITECOPY;

template<DWORD dwAccessRights>
bool CheckAccess(void* pAddress, size_t nSize)
{
	if (!pAddress || !nSize)
	{
		return false;
	}

	MEMORY_BASIC_INFORMATION sMBI;
	bool bRet = false;

	UINT_PTR pCurrentAddress = UINT_PTR(pAddress);
	UINT_PTR pEndAdress = pCurrentAddress + (nSize - 1);

	do
	{
		ZeroMemory(&sMBI, sizeof(sMBI));
		VirtualQuery(LPCVOID(pCurrentAddress), &sMBI, sizeof(sMBI));

		bRet = (sMBI.State & MEM_COMMIT) // memory allocated and
			&& !(sMBI.Protect & dwForbiddenArea) // access to page allowed and
			&& (sMBI.Protect & dwAccessRights); // the required rights

		pCurrentAddress = (UINT_PTR(sMBI.BaseAddress) + sMBI.RegionSize);
	} while (bRet && pCurrentAddress <= pEndAdress);

	return bRet;
}

#define IsBadWritePtr(p,n) (!CheckAccess<dwWriteRights>(p,n))
#define IsBadReadPtr(p,n) (!CheckAccess<dwReadRights>(p,n))
#define IsBadStringPtrW(p,n) (!CheckAccess<dwReadRights>(p,n*2))

BYTE* GetPointerAfterNBytes(void* pAddress, ULONG byteCount)
{
	return ((byte*)pAddress) + byteCount;
}

#define ARRAY_LEN(x)  (sizeof(x)/sizeof(x[0]))

HRESULT ValueReader::GetObjectValue(UINT_PTR startAddress, ULONG length,
	CorElementType elementType, AnyPtr out)
{
	CHAR value[1024];
	INT charCount = ARRAY_LEN(value) - 1;

	switch (elementType)
	{
	case ELEMENT_TYPE_BOOLEAN:
	{
		bool* pBool = reinterpret_cast<bool*>(startAddress);
		if (pBool == NULL) {
			*out = 0; break;
		}
		*out = *pBool;
	}
	break;

	case ELEMENT_TYPE_CHAR:
	{
		WCHAR* pChar = reinterpret_cast<WCHAR*>(startAddress);
		if (pChar == NULL) {
			*out = 0; break;
		}
		*out = *pChar;
	}
	break;

	case ELEMENT_TYPE_I1:
	{
		INT8* pNumber = reinterpret_cast<INT8*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_U1:
	{
		UCHAR* pNumber = reinterpret_cast<UCHAR*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_I2:
	{
		INT16* pNumber = reinterpret_cast<INT16*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_U2:
	{
		UINT16* pNumber = reinterpret_cast<UINT16*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_I4:
	{
		INT32* pNumber = reinterpret_cast<INT32*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_U4:
	{
		UINT32* pNumber = reinterpret_cast<UINT32*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;

	}
	break;

	case ELEMENT_TYPE_I8:
	{
		INT64* pNumber = reinterpret_cast<INT64*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_U8:
	{
		UINT64* pNumber = reinterpret_cast<UINT64*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_R4:
	{
		FLOAT* pNumber = reinterpret_cast<FLOAT*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_R8:
	{
		DOUBLE* pNumber = reinterpret_cast<DOUBLE*>(startAddress);
		if (pNumber == NULL) {
			*out = 0; break;
		}
		*out = *pNumber;
	}
	break;

	case ELEMENT_TYPE_STRING:
	{
		// Look at the reference stored at the given address.
		unsigned __int64* pAddress = (unsigned __int64*)startAddress;
		if (startAddress == NULL) {
			*out = std::wstring();
			break;
		}
		byte* managedReference = (byte*)(*pAddress);

		// Check for null string.
		if (managedReference == NULL)
		{
			*out = std::wstring();
			break;
		}

		ULONG stringLengthOffset = 0;
		ULONG stringBufferOffset = 0;

		HRESULT hr = g_corProfilerInfo->GetStringLayout2(&stringLengthOffset, &stringBufferOffset);
		if (FAILED(hr)) {
			*out = std::wstring();
			break;
		}

		byte* pLength = GetPointerAfterNBytes(managedReference, stringLengthOffset);
		if (IsBadReadPtr(pLength, 8)) {
			*out = std::wstring();
			break;
		}

		ULONG stringLength = *pLength;
		if (stringLength == 0)
		{
			*out = std::wstring();
			break;
		}

		byte* pBuffer = GetPointerAfterNBytes(managedReference, stringBufferOffset);

		::WideCharToMultiByte(CP_ACP, 0, (WCHAR*)pBuffer, stringLength + 1, value,
			charCount, NULL, NULL);
		*out = std::wstring((WCHAR*)pBuffer);
	}
	break;

	case ELEMENT_TYPE_PTR:
	{
		// Look at the reference stored at the given address.
		unsigned __int64* pAddress = (unsigned __int64*)startAddress;
		if (pAddress == NULL) {
			*out = std::wstring();
			break;
		}
		byte* managedReference = (byte*)(*pAddress);

		// Check for null string.
		if (managedReference == NULL)
		{
			*out = std::wstring();
			break;
		}
	}
	break;

	case ELEMENT_TYPE_BYREF:
	{
		// Look at the reference stored at the given address.
		unsigned __int64* pAddress = (unsigned __int64*)startAddress;
		if (pAddress == NULL) {
			*out = std::wstring();
			break;
		}

		byte* managedReference = (byte*)(*pAddress);

		// Check for null string.
		if (managedReference == NULL)
		{
			*out = std::wstring();
			break;
		}
	}
	break;

	case ELEMENT_TYPE_SZARRAY:
	{
		unsigned __int64* pAddress = (unsigned __int64*)startAddress;

		if (startAddress == 0) {
			*out = std::wstring();
			break;
		}
		if (IsBadReadPtr(pAddress, length)) {
			*out = std::wstring();
			break;
		}

		byte* managedReference = (byte*)(*pAddress);

		// Check for null string.
		if (managedReference == NULL)
		{
			*out = std::wstring();
			break;
		}

		// single dimension array so the following arrays only need 1 element to receive size and lower bound
		ULONG32* pDimensionSizes = new ULONG32[1];
		int* pDimensionLowerBounds = new int[1];
		byte* pElements; // will point to the beginning of the array elements

		HRESULT hr = g_corProfilerInfo->GetArrayObjectInfo((ObjectID)managedReference,
			1, pDimensionSizes, pDimensionLowerBounds, &pElements);
		if (FAILED(hr)) {
			*out = std::wstring();
			break;
		}

		ULONG32 arrayLength = pDimensionSizes[0];
		delete pDimensionSizes;
		delete pDimensionLowerBounds;

		if (arrayLength == 0)
		{
			strcpy_s(value, charCount, "empty single dimension array");
			break;
		}

		ClassID classId;
		ULONG rank = 0;
		CorElementType baseElementType;
		hr = g_corProfilerInfo->GetClassFromObject((ObjectID)managedReference, &classId);
		hr = g_corProfilerInfo->IsArrayClass(classId, &baseElementType, NULL, &rank);

		GetArrayValue(pElements, arrayLength, baseElementType, out);
	}
	break;

	case ELEMENT_TYPE_VAR:
	case ELEMENT_TYPE_CLASS:
	case ELEMENT_TYPE_VALUETYPE:
	case ELEMENT_TYPE_OBJECT:
	case ELEMENT_TYPE_I:
	case ELEMENT_TYPE_U:
	case ELEMENT_TYPE_GENERICINST:
	{
		// Look at the reference stored at the given address.
		unsigned __int64* pAddress = (unsigned __int64*)startAddress;
		if (pAddress == NULL) {
			break;
		}

		*out = *pAddress;
	}
	break;

	default:
		printf("unknown type 0x%x", elementType);
		break;
	}

	return S_OK;
}

HRESULT ValueReader::GetElementValue(byte*& pElement, CorElementType elementType,
	AnyPtr out)
{
	HRESULT hr = GetObjectValue((UINT_PTR)pElement, sizeof(void*), elementType, out);
	if (FAILED(hr)) {
		printf("\r\nGetObjectValue failed");
	}

	switch (elementType)
	{
	case ELEMENT_TYPE_BOOLEAN:
		pElement += sizeof(bool);
		break;

	case ELEMENT_TYPE_CHAR:
		pElement += sizeof(WCHAR);
		break;

	case ELEMENT_TYPE_I1:
		pElement += sizeof(char);
		break;

	case ELEMENT_TYPE_U1:
		pElement += sizeof(unsigned char);
		break;

	case ELEMENT_TYPE_I2:
		pElement += sizeof(short);
		break;

	case ELEMENT_TYPE_U2:
		pElement += sizeof(unsigned short);
		break;

	case ELEMENT_TYPE_I4:
		pElement += sizeof(int);
		break;

	case ELEMENT_TYPE_U4:
		pElement += sizeof(unsigned int);
		break;

	case ELEMENT_TYPE_I8:
		pElement += sizeof(long);
		break;

	case ELEMENT_TYPE_U8:
		pElement += sizeof(unsigned long);
		break;

	case ELEMENT_TYPE_R4:
		pElement += sizeof(float);
		break;

	case ELEMENT_TYPE_R8:
		pElement += sizeof(double);
		break;

	case ELEMENT_TYPE_STRING:
		pElement += sizeof(void*);
		break;

	case ELEMENT_TYPE_CLASS:
		// NOTE: can't call GetObjectValue recursively because won't fit on one line
		//sprintf_s(value, charCount, "0x%p", *(UINT_PTR*)pElement);
		pElement += sizeof(void*);
		break;

	case ELEMENT_TYPE_SZARRAY:
		// arrays are reference types so skip the size of an address
		pElement += sizeof(void*);
		break;

	case ELEMENT_TYPE_OBJECT:
		//strcpy_s(value, charCount, "obj");
		pElement += sizeof(void*);
		break;

	default:
		printf("unknown type objectvalue 0x%x", elementType);
		return E_FAIL;
	}

	return S_OK;
}

HRESULT ValueReader::GetArrayValue(byte*& pElement, ULONG32 arrayLength,
	CorElementType elementType, AnyPtr out)
{
	std::shared_ptr<std::vector<AnyPtr>> v = std::make_shared<std::vector<AnyPtr>>();

	for (ULONG current = 0; current < arrayLength; current++)
	{
		AnyPtr arrayValue = std::make_shared<std::any>();
		HRESULT hr = GetElementValue(pElement, elementType, arrayValue);
		if (FAILED(hr)) {
			printf ("\r\nGetElementValue failed");
		}

		(*v).emplace_back(arrayValue);
	}

	*out = v;

	return S_OK;
}