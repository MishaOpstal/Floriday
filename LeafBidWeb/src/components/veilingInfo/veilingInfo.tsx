import s from '@/components/veilingInfo/velinginfo.module.css';

type BigFieldProps = {
    naam: string;
    prijs: number;
    plaatje: string;
    info: string;
    oogst: string;
    leverancier: string;
    regio: string;
    aantal: number;
};

export default function BigInfoVeld({
    naam,
    prijs,
    plaatje,
    info,
    oogst,
    leverancier,
    regio,
    aantal,
}: BigFieldProps) {
    const imageSrc = `/${plaatje}`;

    return (
        <div
            className={`d-flex flex-column p-4 w-100 h-100 text-black bg-white ${s.wrapper}`}>
            <div className="d-flex flex-row gap-4">
            <img
                src={imageSrc}
                alt={naam}
                className={`mb-3 ${s.plaatje}`}
            />
                <div className={`d-flex flex-row gap-1 ${s.infoBox}`}>
                    <p className="m-0" style={{ fontSize: '21px', fontWeight: 400 }}>{info}</p></div>
                </div>
            <div className={`d-flex flex-column gap-3 p-3 ${s.tekstcontainer}`}>
                <h2 className="m-0" style={{ fontSize: '24px', fontWeight: 600 }}>{naam}</h2>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{aantal}</p>
                <p className="m-0 text-decoration-line-through" style={{ fontSize: '16px', fontWeight: 400, opacity: 0.8 }}>{prijs}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{oogst}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{leverancier}</p>
                <p className="m-0" style={{ fontSize: '16px', fontWeight: 400 }}>{regio}</p>

            </div>
            <button
                className={`btn mt-3 align-self-start ${s.knop}`}
            >
                Bied nu
            </button>

        </div>
    );
}
