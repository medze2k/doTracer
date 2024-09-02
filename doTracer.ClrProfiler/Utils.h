#pragma once

#include <string>
#include <algorithm>

enum class HookType { enterHook = 0, exitHook, tailcallHook };

std::wstring stringifyHookType(HookType val);
std::string ws2s(const std::wstring& wstr);
