import { Audiowide } from 'next/font/google'
import Link from "next/link"

const audiowide = Audiowide({
  subsets: ['latin'],
  weight: '400',
})

export default function Home() {

  const botaoClasse ="shadow-xl/30 shadow-[#12698A]  border-4 border-[#12698A] w-60 md:w-80 text-2xl md:text-3xl font-bold  py-3 md:py-4 rounded-md hover:bg-[#12698A] duration-150 text-center -mb-7";
  const titulo =`text-7xl mt-25 md:text-9xl italic font-bold ${audiowide.className} relative inline-block`;

  return (
    <section >
      <div className="text-center">
        <div>
          <h1 className={titulo}>ControlAí<span data-aos="zoom-in" className="absolute bottom-0 left-1 w-full h-2 bg-gradient-to-r from-[#00d4ff] via-[#12698A] to-[#00d4ff] rounded-full"></span></h1>
        </div>

        <div className="mt-15 flex justify-center">
          <Link href="perfil" className={botaoClasse}>Movimentos Fixos</Link>
        </div>

        <div className="mt-15 flex justify-center">
          <Link href="extratos" className={botaoClasse}>Extratos</Link>
        </div>

        <div className="mt-15 flex justify-center">
          <Link href="configuracao" className={botaoClasse}>Configuração</Link>
        </div>
      </div>
    </section>
  );
}
