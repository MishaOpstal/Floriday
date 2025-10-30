'use client';

import React from 'react';
import s from './dashboardPanel.module.css';

type DashboardPanelProps = {
    title: string;
    imageSrc?: string;
    veilingduur: string;
    totaalprijs: string;
    kloklocatie: string;
    children?: React.ReactNode;
};

const DashboardPanel: React.FC<DashboardPanelProps> = ({   title, imageSrc, veilingduur, totaalprijs, kloklocatie, children, }) => {
    return (
        <div className={s.panel}>
            <div className={s.left}>
                {imageSrc && <img src={imageSrc} alt="Afbeelding" className={s.image} />}
            </div>

            <div className={s.middle}>
                <div className={s.title}>{title}</div>
                <div className={s.content}>
                    <p><strong>Veilingduur:</strong> {veilingduur}</p>
                    <p><strong>Totaalprijs:</strong> {totaalprijs}</p>
                    <p><strong>Kloklocatie:</strong> {kloklocatie}</p>
                </div>
            </div>

            <div className={s.right}>
                {children}
            </div>
        </div>
    );
};

export default DashboardPanel;
