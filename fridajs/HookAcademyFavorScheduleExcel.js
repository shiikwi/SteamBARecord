'use strict'

const moduleName = "GameAssembly.dll";
const constructorRva = ptr("0x1FD18C0");
const module = Process.findModuleByName(moduleName);

if(module){
    const getHook = [
            {name: "GetRootAsAcademyFavorScheduleExcelTable()", rva: 0x1f1cfd0},
            {name: "get_Id()", rva: 0x1f1d590},
            {name: "get_CharacterId()", rva: 0x1f1d660}
    ];

    getHook.forEach(getter =>
    {
        const targetRVA = ptr(getter.rva);
        const targetAddress = module.base.add(targetRVA);
        console.log(`* TargetAddress: ${targetAddress}`);

        Interceptor.attach(targetAddress, {
            onEnter: function(args){
                this.instanceHandle = args[0];

                const currentPos = this.instanceHandle.add(0x18).readInt();
                console.log(`* pos Position: ${currentPos}`);
            },

            onLeave: function(retval){
                const reValue = retval.toInt32();

                console.log(`* ${getter.name} instance: ${this.instanceHandle} Value: ${reValue}`);
            }
        });
    })
}else{
    console.log("Hook Failed");
}