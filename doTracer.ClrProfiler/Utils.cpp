
static const wchar_t* HookTypeString[] =
{ L"Enter", L"Exit", L"Tailcall" };
#include "Utils.h"

std::wstring stringifyHookType(HookType val)
{
    int i = (int)val;
    std::wstring s(HookTypeString[i]);
    return s;
}

std::string ws2s(const std::wstring& wide)
{
    std::string str(wide.length(), 0);
    std::transform(wide.begin(), wide.end(), str.begin(), [](wchar_t c) {
        return (char)c;
    });
    return str;
}
