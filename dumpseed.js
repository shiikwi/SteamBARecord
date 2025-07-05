'use strict'

const functionRVA = ptr(0x0C0FC30)
const module = Process.getModuleByName("GameAssembly.dll");

if(module){
    const baseAddress = module.base;
    const targetAddress = baseAddress.add(functionRVA);

    console.log(`* TargetAddress: ${targetAddress}`);

Interceptor.attach(targetAddress, {
    onEnter: function(args){
    //     const keyHandle = args[0];

    //     let KeyLength = keyHandle.add(0x10).readInt();
    //     let key = keyHandle.add(0x14).readUtf16String(KeyLength);

    //     console.log(`* Hash key: ${key}`);
    //     this.inputkey = key;

    console.log(`* hash object: ${args[0]}`);
    },

    onLeave: function(retval){
        const seed = retval.toUInt32();
        // console.log(`* ${this.inputkey} seed : ${seed}`);
        console.log(`* seed: ${seed}`);
    }
})
}else{
    console.error("Hook Failed");
}