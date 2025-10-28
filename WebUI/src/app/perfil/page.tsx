import { AlertDialog, AlertDialogAction, AlertDialogContent, AlertDialogDescription, AlertDialogFooter, AlertDialogTitle, AlertDialogTrigger } from "@/components/ui/alert-dialog";
import EntradaFixaCard from "./entradaFixaCard";
import SaidaFixaCard from "./saidaFixaCard";
import { Button } from "@/components/ui/button";

export default function  MeuPerfil(){
    
    return (
        <div className="relative mt-[80px]">
            <div className="mt-8 absolute left-1/2 -translate-x-1/2">
                {/* <AlertDialog>
                    <AlertDialogTrigger>
                        <Button variant="ghost" className="bg-[#12698a] text-black font-bold rounded-full">?</Button>
                    </AlertDialogTrigger>
                    <AlertDialogContent>
                        <AlertDialogTitle>
                            Ajuda de Página.
                        </AlertDialogTitle>
                        <AlertDialogDescription>
                            Nesta página você pode criar movimentos fixos mensais. Se houver um gasto ou ganho recorrente, facilite o processo configurando previamente como uma <span className="font-bold">Entrada</span> ou <span className="font-bold">Saída</span>.
                        </AlertDialogDescription>
                        <AlertDialogFooter>
                            <AlertDialogAction className="bg-[#12698A] text-black">
                                Ok!
                            </AlertDialogAction>
                        </AlertDialogFooter>
                    </AlertDialogContent>
                </AlertDialog> */}
            </div>
            <div className="grid grid-cols-1 lg:grid-cols-2">
                <EntradaFixaCard/>
                <SaidaFixaCard/>
            </div>
        </div>
    )  
}