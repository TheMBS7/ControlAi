
import { Button } from "@/components/ui/button"
import CategoriaCard from "./categoriaCard"
import PessoasCard from "./pessoasCard"
import { AlertDialog, AlertDialogTrigger, AlertDialogContent, AlertDialogTitle, AlertDialogDescription, AlertDialogAction, AlertDialogFooter } from "@/components/ui/alert-dialog"

export default function Configuracao() {
  return (
    <div className="relative mt-[80px]">
      {/* <div className=" absolute mt-8 left-1/2 -translate-x-1/2">
        <AlertDialog>
          <AlertDialogTrigger>
            <Button variant="ghost" className="bg-[#12698a] text-black font-bold rounded-full">?</Button>
          </AlertDialogTrigger>
          <AlertDialogContent>
            <AlertDialogTitle>
              Ajuda de Página.
            </AlertDialogTitle>
            <AlertDialogDescription>
              Esta página permite gerenciar os campos usados nos extratos. Aqui, você pode criar e editar <span className="font-bold">Categorias</span> e <span className="font-bold">Pessoas</span>, que serão utilizados para organizar melhor suas movimentações.
            </AlertDialogDescription>
            <AlertDialogFooter>
              <AlertDialogAction className="bg-[#12698A] text-black">
                Ok!
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
      </div> */}
      <div className="grid justify-items-center grid-cols-1 lg:grid-cols-2">
        <CategoriaCard />
        <PessoasCard />
      </div>
    </div>
  )
}
