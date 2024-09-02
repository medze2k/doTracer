#pragma once

#include "cor.h"
#include "corprof.h"

#include <string>
#include <any>
#include <vector>
#include <memory>

typedef std::shared_ptr<std::any> AnyPtr;

class ValueReader final {

public:

	static HRESULT GetObjectValue(UINT_PTR startAddress, ULONG length, CorElementType elementType,
		AnyPtr out);

	static HRESULT GetElementValue(byte*& pElement, CorElementType elementType,
		AnyPtr out);

	static HRESULT GetArrayValue(byte*& pElement, ULONG32 arrayLength,
		CorElementType elementType, AnyPtr out);

private:
};
