'use strict';

const functionRVA = ptr(0x1821280)
const module = Process.getModuleByName("GameAssembly.dll");

if(module){
    const baseAddress = module.base;
    const targetAddress = baseAddress.add(functionRVA);

    console.log(`* TargetAddress: ${targetAddress}`);

Interceptor.attach(targetAddress, {
    onEnter: function(args){
        const keyHandle = args[0];
        const length = args[1].toInt32();

        let KeyLength = keyHandle.add(0x10).readInt();
        let key = keyHandle.add(0x14).readUtf16String(KeyLength);

        console.log(`* Dumped key: ${key}`);
        console.log(`* Dumped length: ${length}`);
        this.inputkey = key;
    },

    onLeave: function(retval){
        const passwordHandle = retval;
        const passwordLength = passwordHandle.add(0x10).readInt();
        let password = passwordHandle.add(0x14).readUtf16String(passwordLength);

        console.log(`* Dumped password: ${this.inputkey}: ${password}`);
    }
})

}else{
    console.error("Hook Failed");
}