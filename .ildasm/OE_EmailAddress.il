
//  Microsoft (R) .NET Framework IL Disassembler.  Version 4.8.3928.0
//  Copyright (c) Microsoft Corporation.  All rights reserved.



// Metadata version: v2.0.50727
.assembly extern Activant.P21.Extensions
{
  .publickeytoken = (A3 19 98 6F D9 C8 53 C7 )                         // ...o..S.
  .ver 1:0:0:0
}
.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 2:0:0:0
}
.assembly OE_EmailAddress
{
  .custom instance void [mscorlib]System.Reflection.AssemblyTitleAttribute::.ctor(string) = ( 01 00 0F 4F 45 5F 45 6D 61 69 6C 41 64 64 72 65   // ...OE_EmailAddre
                                                                                              73 73 00 00 )                                     // ss..
  .custom instance void [mscorlib]System.Reflection.AssemblyDescriptionAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyConfigurationAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyCompanyAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Reflection.AssemblyProductAttribute::.ctor(string) = ( 01 00 0F 4F 45 5F 45 6D 61 69 6C 41 64 64 72 65   // ...OE_EmailAddre
                                                                                                73 73 00 00 )                                     // ss..
  .custom instance void [mscorlib]System.Reflection.AssemblyCopyrightAttribute::.ctor(string) = ( 01 00 12 43 6F 70 79 72 69 67 68 74 20 C2 A9 20   // ...Copyright .. 
                                                                                                  20 32 30 31 35 00 00 )                            //  2015..
  .custom instance void [mscorlib]System.Reflection.AssemblyTrademarkAttribute::.ctor(string) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.ComVisibleAttribute::.ctor(bool) = ( 01 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.InteropServices.GuidAttribute::.ctor(string) = ( 01 00 24 35 63 63 63 37 32 35 39 2D 30 37 39 38   // ..$5ccc7259-0798
                                                                                                  2D 34 35 38 33 2D 61 64 61 37 2D 30 33 64 63 33   // -4583-ada7-03dc3
                                                                                                  35 35 64 39 65 32 33 00 00 )                      // 55d9e23..
  .custom instance void [mscorlib]System.Reflection.AssemblyFileVersionAttribute::.ctor(string) = ( 01 00 07 31 2E 30 2E 30 2E 30 00 00 )             // ...1.0.0.0..
  .custom instance void [mscorlib]System.Security.AllowPartiallyTrustedCallersAttribute::.ctor() = ( 01 00 00 00 ) 

  // --- The following custom attribute is added automatically, do not uncomment -------
  //  .custom instance void [mscorlib]System.Diagnostics.DebuggableAttribute::.ctor(valuetype [mscorlib]System.Diagnostics.DebuggableAttribute/DebuggingModes) = ( 01 00 07 01 00 00 00 00 ) 

  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilationRelaxationsAttribute::.ctor(int32) = ( 01 00 08 00 00 00 00 00 ) 
  .custom instance void [mscorlib]System.Runtime.CompilerServices.RuntimeCompatibilityAttribute::.ctor() = ( 01 00 01 00 54 02 16 57 72 61 70 4E 6F 6E 45 78   // ....T..WrapNonEx
                                                                                                             63 65 70 74 69 6F 6E 54 68 72 6F 77 73 01 )       // ceptionThrows.
  .hash algorithm 0x00008004
  .ver 1:0:0:0
}
.module OE_EmailAddress.dll
// MVID: {F4D0504C-C9C9-45C3-9765-DDA7EEF3F92D}
.imagebase 0x00400000
.file alignment 0x00000200
.stackreserve 0x00100000
.subsystem 0x0003       // WINDOWS_CUI
.corflags 0x00000001    //  ILONLY
// Image base: 0x00000147D18B0000


// =============== CLASS MEMBERS DECLARATION ===================

.class public auto ansi beforefieldinit KomarBR.Rules.OE_EmailAddress
       extends [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule
{
  .method public hidebysig virtual instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult 
          Execute() cil managed
  {
    // Code size       121 (0x79)
    .maxstack  2
    .locals init (class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult V_0,
             string V_1,
             class [mscorlib]System.Exception V_2,
             class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult V_3,
             bool V_4)
    IL_0000:  nop
    IL_0001:  newobj     instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::.ctor()
    IL_0006:  stloc.0
    IL_0007:  ldloc.0
    IL_0008:  ldc.i4.1
    IL_0009:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
    IL_000e:  nop
    .try
    {
      IL_000f:  nop
      IL_0010:  ldarg.0
      IL_0011:  call       instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule::get_Data()
      IL_0016:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_001b:  ldstr      "email_address"
      IL_0020:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::get_Item(string)
      IL_0025:  callvirt   instance string [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::get_FieldValue()
      IL_002a:  stloc.1
      IL_002b:  ldloc.1
      IL_002c:  call       bool [mscorlib]System.String::IsNullOrEmpty(string)
      IL_0031:  stloc.s    V_4
      IL_0033:  ldloc.s    V_4
      IL_0035:  brtrue.s   IL_0055

      IL_0037:  nop
      IL_0038:  ldarg.0
      IL_0039:  call       instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule::get_Data()
      IL_003e:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataCollection::get_Fields()
      IL_0043:  ldstr      "ship2_email_address"
      IL_0048:  callvirt   instance class [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataFields::GetFieldByAlias(string)
      IL_004d:  ldloc.1
      IL_004e:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.DataField::set_FieldValue(string)
      IL_0053:  nop
      IL_0054:  nop
      IL_0055:  nop
      IL_0056:  leave.s    IL_0072

    }  // end .try
    catch [mscorlib]System.Exception 
    {
      IL_0058:  stloc.2
      IL_0059:  nop
      IL_005a:  ldloc.0
      IL_005b:  ldloc.2
      IL_005c:  callvirt   instance string [mscorlib]System.Exception::get_Message()
      IL_0061:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Message(string)
      IL_0066:  nop
      IL_0067:  ldloc.0
      IL_0068:  ldc.i4.0
      IL_0069:  callvirt   instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.RuleResult::set_Success(bool)
      IL_006e:  nop
      IL_006f:  nop
      IL_0070:  leave.s    IL_0072

    }  // end handler
    IL_0072:  nop
    IL_0073:  ldloc.0
    IL_0074:  stloc.3
    IL_0075:  br.s       IL_0077

    IL_0077:  ldloc.3
    IL_0078:  ret
  } // end of method OE_EmailAddress::Execute

  .method public hidebysig virtual instance string 
          GetName() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "OE Email Address"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method OE_EmailAddress::GetName

  .method public hidebysig virtual instance string 
          GetDescription() cil managed
  {
    // Code size       11 (0xb)
    .maxstack  1
    .locals init (string V_0)
    IL_0000:  nop
    IL_0001:  ldstr      "Copy the Email Adress from Contacts tab to Ship-to"
    + " Tab"
    IL_0006:  stloc.0
    IL_0007:  br.s       IL_0009

    IL_0009:  ldloc.0
    IL_000a:  ret
  } // end of method OE_EmailAddress::GetDescription

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       7 (0x7)
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [Activant.P21.Extensions]Activant.P21.Extensions.BusinessRule.Rule::.ctor()
    IL_0006:  ret
  } // end of method OE_EmailAddress::.ctor

} // end of class KomarBR.Rules.OE_EmailAddress


// =============================================================

// *********** DISASSEMBLY COMPLETE ***********************
// WARNING: Created Win32 resource file C:\Users\DanShao\.vscode\p21_business_rules\.ildasm\OE_EmailAddress.res
