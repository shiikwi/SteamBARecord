'use strict';

const moduleName = "GameAssembly.dll";

const keyFuncRva = ptr("0x117EEF0"); 

const module = Process.findModuleByName(moduleName);

if (module) {
    const targetAddress = module.base.add(keyFuncRva);
    console.log(`* TargetAddres: ${targetAddress}`);
    
    Interceptor.attach(targetAddress, {

        onEnter: function(args) {
            const nameHandle = args[0];

            let nameLength = nameHandle.add(0x10).readInt();
            let key = nameHandle.add(0x14).readUtf16String(nameLength);

            console.log(`* dump name: ${key}`)
        }
    });


} else {
    console.error(`Hook Failed`);
}