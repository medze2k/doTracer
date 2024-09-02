#include "ParameterInfo.h"
#include "Utils.h"

extern ICorProfilerInfo8* g_corProfilerInfo;

ParameterInfo::ParameterInfo() :
    m_ParamAttributes(0),
    m_ParamDefToken(0),
    m_ParamTypeDefToken(0),
	m_ParamType(ELEMENT_TYPE_END),
	m_ParamName(MAX_LENGTH, 0),
    m_pMetaDataImport(nullptr)
{
	m_ParamTypeAsString.reserve(MAX_LENGTH);
}

ParameterInfo::ParameterInfo(IMetaDataImport2* pMetaDataImport) :
	m_ParamAttributes(0),
	m_ParamDefToken(0),
	m_ParamTypeDefToken(0),
	m_ParamType(ELEMENT_TYPE_END),
	m_ParamName(MAX_LENGTH, 0),
	m_pMetaDataImport(pMetaDataImport)
{
	m_ParamTypeAsString.reserve(MAX_LENGTH);
}

ParameterInfo::ParameterInfo(IMetaDataImport2* pMetaDataImport, mdParamDef ParamDef) :
	m_ParamAttributes(0),
    m_ParamDefToken(ParamDef),
    m_ParamTypeDefToken(0),
	m_ParamType(ELEMENT_TYPE_END),
	m_ParamName(MAX_LENGTH, 0),
    m_pMetaDataImport(pMetaDataImport)
{
    ULONG ParameterNameLength = 0;
	m_ParamTypeAsString.reserve(MAX_LENGTH);

    m_pMetaDataImport->GetParamProps(
        ParamDef, NULL, NULL, &m_ParamName[0], MAX_LENGTH, &ParameterNameLength,
        &m_ParamAttributes, NULL, NULL, NULL);
	m_ParamName.resize(ParameterNameLength);
}

const std::wstring ParameterInfo::Name() const
{
    return m_ParamName;
}

const std::wstring ParameterInfo::TypeAsString() const
{
	return m_ParamTypeAsString;
}

const mdParamDef ParameterInfo::DefToken() const
{
	return m_ParamDefToken;
}

const DWORD ParameterInfo::Attributes() const
{
	return m_ParamAttributes;;
}

const CorElementType ParameterInfo::Type() const
{
	return m_ParamType;;
}

const AnyPtr ParameterInfo::Value() const
{
	return m_ParamValue;
}

PCCOR_SIGNATURE ParameterInfo::ParseSignature(PCCOR_SIGNATURE pSigBlob)
{
	COR_SIGNATURE corSignature = *pSigBlob++;

	if (m_ParamType == ELEMENT_TYPE_END) {
		m_ParamType = static_cast<CorElementType>(corSignature);
	}

	switch (corSignature)
	{
	case ELEMENT_TYPE_VOID:
		m_ParamTypeAsString += L"void";
		break;
	case ELEMENT_TYPE_BOOLEAN:
		m_ParamTypeAsString += L"bool";
		break;
	case ELEMENT_TYPE_CHAR:
		m_ParamTypeAsString += L"char";
		break;
	case ELEMENT_TYPE_I1:
		m_ParamTypeAsString += L"int8";
		break;
	case ELEMENT_TYPE_U1:
		m_ParamTypeAsString += L"unsigned int8";
		break;
	case ELEMENT_TYPE_I2:
		m_ParamTypeAsString += L"int16";
		break;
	case ELEMENT_TYPE_U2:
		m_ParamTypeAsString += L"unsigned int16";
		break;
	case ELEMENT_TYPE_I4:
		m_ParamTypeAsString += L"int32";
		break;
	case ELEMENT_TYPE_U4:
		m_ParamTypeAsString += L"unsigned int32";
		break;
	case ELEMENT_TYPE_I8:
		m_ParamTypeAsString += L"int64";
		break;
	case ELEMENT_TYPE_U8:
		m_ParamTypeAsString += L"unsigned int64";
		break;
	case ELEMENT_TYPE_R4:
		m_ParamTypeAsString += L"float32";
		break;
	case ELEMENT_TYPE_R8:
		m_ParamTypeAsString += L"float64";
		break;
	case ELEMENT_TYPE_STRING:
		m_ParamTypeAsString += L"String";
		break;
	case ELEMENT_TYPE_VAR:
		m_ParamTypeAsString += L"class variable(unsigned int8)";
		break;
	case ELEMENT_TYPE_MVAR:
		m_ParamTypeAsString += L"method variable(unsigned int8)";
		break;
	case ELEMENT_TYPE_TYPEDBYREF:
		m_ParamTypeAsString += L"refany";
		break;
	case ELEMENT_TYPE_I:
		m_ParamTypeAsString += L"int";
		break;
	case ELEMENT_TYPE_U:
		m_ParamTypeAsString += L"unsigned int";
		break;
	case ELEMENT_TYPE_OBJECT:
		m_ParamTypeAsString += L"Object";
		break;
	case ELEMENT_TYPE_SZARRAY:
		pSigBlob = ParseSignature(pSigBlob);
		m_ParamTypeAsString += L"[]";
		break;
	case ELEMENT_TYPE_PINNED:
		pSigBlob = ParseSignature(pSigBlob);
		m_ParamTypeAsString += L"pinned";
		break;
	case ELEMENT_TYPE_PTR:
		pSigBlob = ParseSignature(pSigBlob);
		m_ParamTypeAsString += L"*";
		break;
	case ELEMENT_TYPE_BYREF:
		pSigBlob = ParseSignature(pSigBlob);
		m_ParamTypeAsString += L"&";
		break;
	case ELEMENT_TYPE_VALUETYPE:
	case ELEMENT_TYPE_CLASS:
	case ELEMENT_TYPE_CMOD_REQD:
	case ELEMENT_TYPE_CMOD_OPT:
	{
		mdToken	token;
		pSigBlob += CorSigUncompressToken(pSigBlob, &token);

		WCHAR className[MAX_LENGTH];
		if (TypeFromToken(token) == mdtTypeRef)
		{
			m_pMetaDataImport->GetTypeRefProps(token, NULL, className, MAX_LENGTH, NULL);
		}
		else
		{
			m_pMetaDataImport->GetTypeDefProps(token, className, MAX_LENGTH, NULL, NULL, NULL);
		}

		m_ParamTypeAsString += className;
	}
	break;
	case ELEMENT_TYPE_GENERICINST:
	{
		pSigBlob = ParseSignature(pSigBlob);

		m_ParamTypeAsString += L"<";
		ULONG arguments = CorSigUncompressData(pSigBlob);
		for (ULONG i = 0; i < arguments; ++i)
		{
			if (i != 0)
			{
				m_ParamTypeAsString += L", ";
			}

			pSigBlob = ParseSignature(pSigBlob);
		}
		m_ParamTypeAsString += L">";
	}
	break;
	case ELEMENT_TYPE_ARRAY:
	{
		pSigBlob = ParseSignature(pSigBlob);
		ULONG rank = CorSigUncompressData(pSigBlob);
		if (rank == 0)
		{
			m_ParamTypeAsString += L"[?]";
		}
		else
		{
			ULONG arraysize = (sizeof(ULONG) * 2 * rank);
			ULONG* lower = (ULONG*)_alloca(arraysize);
			memset(lower, 0, arraysize);
			ULONG* sizes = &lower[rank];

			ULONG numsizes = CorSigUncompressData(pSigBlob);
			for (ULONG i = 0; i < numsizes && i < rank; i++)
			{
				sizes[i] = CorSigUncompressData(pSigBlob);
			}

			ULONG numlower = CorSigUncompressData(pSigBlob);
			for (ULONG i = 0; i < numlower && i < rank; i++)
			{
				lower[i] = CorSigUncompressData(pSigBlob);
			}

			m_ParamTypeAsString += L"[";
			for (ULONG i = 0; i < rank; ++i)
			{
				if (i > 0)
				{
					m_ParamTypeAsString += L",";
				}

				if (lower[i] == 0)
				{
					if (sizes[i] != 0)
					{
						WCHAR* size = new WCHAR[MAX_LENGTH];
						size[0] = '\0';
						wsprintf(size, L"%d", sizes[i]);
						m_ParamTypeAsString += size;
					}
				}
				else
				{
					WCHAR* low = new WCHAR[MAX_LENGTH];
					low[0] = '\0';
					wsprintf(low, L"%d", lower[i]);
					m_ParamTypeAsString += low;
					m_ParamTypeAsString += L"...";

					if (sizes[i] != 0)
					{
						WCHAR* size = new WCHAR[MAX_LENGTH];
						size[0] = '\0';
						wsprintf(size, L"%d", (lower[i] + sizes[i] + 1));
						m_ParamTypeAsString += size;
					}
				}
			}
			m_ParamTypeAsString += L"]";
		}
	}
	break;
	default:
	case ELEMENT_TYPE_END:
	case ELEMENT_TYPE_SENTINEL:
		WCHAR* elementType = new WCHAR[MAX_LENGTH];
		elementType[0] = '\0';
		wsprintf(elementType, L"<UNKNOWN:0x%X>", corSignature);
		m_ParamTypeAsString += elementType;
		break;
	}

	return pSigBlob;
}

JSON_OBJECT ParameterInfo::Serialize() const {

	AnyPtr value = m_ParamValue;

	auto obj = json::object{
		{"name",  ws2s(m_ParamName).c_str() },
		{"type", ws2s(m_ParamTypeAsString).c_str() },
	};

	if (value->type() == typeid(bool)) {
		bool v = std::any_cast<bool>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(WCHAR)) {
		WCHAR v = std::any_cast<WCHAR>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(INT8)) {
		INT8 v = std::any_cast<INT8>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(UCHAR)) {
		UCHAR v = std::any_cast<UCHAR>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(INT16)) {
		INT16 v = std::any_cast<INT16>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(UINT16)) {
		UINT16 v = std::any_cast<UINT16>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(INT32)) {
		INT32 v = std::any_cast<INT32>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(UINT32)) {
		UINT32 v = std::any_cast<UINT32>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(INT64)) {
		INT64 v = std::any_cast<INT64>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(UINT64)) {
		UINT64 v = std::any_cast<UINT64>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(FLOAT)) {
		FLOAT v = std::any_cast<FLOAT>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(DOUBLE)) {
		DOUBLE v = std::any_cast<DOUBLE>(*value);
		obj.insert("value", json::value(v));
	}
	else if (value->type() == typeid(std::wstring)) {
		std::wstring v = std::any_cast<std::wstring>(*value);
		obj.insert("value", json::value(ws2s(v).c_str()));

	}

	// vectors
	else if (value->type() == typeid(std::shared_ptr<std::vector<AnyPtr>>)) {

		std::shared_ptr<std::vector<AnyPtr>> vec =
			std::any_cast<std::shared_ptr<std::vector<AnyPtr>>>(*value);
		auto j = json::array{};

		for (const auto& e : *vec) {
			if (e->type() == typeid(WCHAR)) {
				WCHAR v = std::any_cast<WCHAR>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(INT8)) {
				INT8 v = std::any_cast<INT8>(*e);
				j.push_back(v);
			}
			else if (value->type() == typeid(UCHAR)) {
				UCHAR v = std::any_cast<UCHAR>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(INT16)) {
				INT16 v = std::any_cast<INT16>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(UINT16)) {
				UINT16 v = std::any_cast<UINT16>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(INT32)) {
				INT32 v = std::any_cast<INT32>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(UINT32)) {
				UINT32 v = std::any_cast<UINT32>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(INT64)) {
				INT64 v = std::any_cast<INT64>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(UINT64)) {
				UINT64 v = std::any_cast<UINT64>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(FLOAT)) {
				FLOAT v = std::any_cast<FLOAT>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(DOUBLE)) {
				DOUBLE v = std::any_cast<DOUBLE>(*e);
				j.push_back(v);
			}
			else if (e->type() == typeid(std::wstring)) {
				std::wstring v = std::any_cast<std::wstring>(*e);
				j.push_back(ws2s(v).c_str());
			}
		}

		obj.insert("value", json::value(j));

	}

	return obj;
}

VOID ParameterInfo::SetParamValue(const AnyPtr v)
{
	m_ParamValue = v;
}
