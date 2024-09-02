#pragma once

#include <string>
#include <any>

#include "cor.h"
#include "corprof.h"
#include "JsonEncoder.h"

#define MAX_LENGTH 1024

typedef json::object JSON_OBJECT;
typedef std::shared_ptr<std::any> AnyPtr;


class ParameterInfo final {

public:
	ParameterInfo();
	ParameterInfo(IMetaDataImport2* pMetaDataImport);
	ParameterInfo(IMetaDataImport2* pMetaDataImport, mdParamDef ParamDef);
	PCCOR_SIGNATURE ParseSignature(PCCOR_SIGNATURE pSigBlob);
	const mdParamDef DefToken() const;
	const DWORD Attributes() const;
	const CorElementType Type() const;
	const std::wstring Name() const;
	const std::wstring TypeAsString() const;
	const AnyPtr Value() const;
	JSON_OBJECT Serialize() const;
	VOID SetParamValue(const AnyPtr v);

private:
	std::wstring m_ParamName;
	AnyPtr m_ParamValue;
	std::wstring m_ParamTypeAsString;
	CorElementType m_ParamType;
	DWORD m_ParamAttributes;
	mdParamDef m_ParamDefToken;
	mdTypeDef m_ParamTypeDefToken;
	IMetaDataImport2* m_pMetaDataImport;
};